using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Runtime.ExceptionServices;

namespace RaspberryPi.Camera
{
    public sealed class RaspiStillImageProvider : ICameraStillImageProvider, IDisposable
    {
        private readonly ILogger<ICameraStillImageProvider> m_logger;
        private readonly SemaphoreSlim m_sem;

        public RaspiStillImageProvider(ILogger<ICameraStillImageProvider> logger)
        {
            this.m_logger = logger;
            this.m_sem = new SemaphoreSlim(1);
        }

        public void Dispose()
        {
            this.m_sem.Dispose();
        }

        public async Task<byte[]> CaptureImage(CaptureImageConfig parameters)
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            this.m_logger.LogDebug("Called captureImage with {0}", parameters);
            Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>> state = new Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>>(parameters, tcs);
            ThreadPool.QueueUserWorkItem(this.CaptureImage, state);
            await this.m_sem.WaitAsync();
            try
            {
                return await tcs.Task;
            }
            catch (Exception ex)
            {
                this.m_logger.LogError(ex, "Error capturing image");
                ExceptionDispatchInfo.Capture(ex).Throw();
                return null;
            }
            finally
            {
                this.m_sem.Release();
            }
        }

        private void CaptureImage(object state)
        {
            this.m_logger.LogDebug("Threadpool");
            Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>> args = (Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>>)state;
            CaptureImageConfig raspiArge = args.Item1;
            TaskCompletionSource<byte[]> tcs = args.Item2;
            try
            {
                string filename = Path.GetTempFileName();

                ProcessStartInfo procStart = new ProcessStartInfo("raspistill");
                procStart.RedirectStandardError = true;
                procStart.RedirectStandardInput = true;
                procStart.RedirectStandardOutput = true;
                raspiArge.OutputFile = filename;
                procStart.Arguments = raspiArge.ToString();

                this.m_logger.LogDebug("raspistill {0}", raspiArge);

                using (Process proc = new Process())
                {
                    proc.StartInfo = procStart;
                    //proc.Exited += (s, e) =>
                    //{

                    //};
                    //proc.ErrorDataReceived += (s, e) =>
                    //{
                    //    this.m_logger.LogWarning("raspistill err: {0}", e.Data);
                    //};
                    //proc.OutputDataReceived += (s, e) =>
                    //{
                    //    this.m_logger.LogInformation("raspistill out: {0}", e.Data);
                    //};

                    if (!proc.Start())
                    {
                        this.m_logger.LogError("Process start failed");
                        throw new InvalidOperationException("Can't start process");
                    }
                    this.m_logger.LogDebug("proc.Start ok");
                    proc.WaitForExit();
                    this.m_logger.LogDebug("proc.WaitForExit ok");

                    this.m_logger.LogDebug("Raspistill exited with code {0}", proc.ExitCode);
                    try
                    {
                        if (proc.ExitCode != 0)
                        {
                            tcs.SetException(new InvalidProgramException($"Error occured, process return with code {proc.ExitCode}"));
                        }
                        else
                        {
                            using (FileStream fs = File.OpenRead(filename))
                            using (MemoryStream ms = new MemoryStream())
                            {
                                fs.CopyTo(ms);
                                tcs.TrySetResult(ms.ToArray());
                            }
                        }
                    }
                    finally
                    {
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (Exception ex)
                        {
                            this.m_logger.LogWarning(ex, "Can't delete temp file");
                            Console.Error.WriteLine("Can't delete temp file " + filename);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.m_logger.LogError(ex, "Error in raspistill execution");
                tcs.TrySetException(ex);
            }
        }
    }
}
