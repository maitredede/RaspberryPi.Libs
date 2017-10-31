using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPi.LibNFC
{
    public static class NfcInitiatorExtensions
    {
        public static Task<NfcTarget> PollAsync(this INfcInitiator initiator, NfcModulation[] modulations, byte period, CancellationToken cancellationToken)
        {
            TaskCompletionSource<NfcTarget> tcs = new TaskCompletionSource<NfcTarget>();
            var data = new PollData
            {
                Initiator = (NfcDevice)initiator,
                Modulations = modulations,
                Period = period,
                CancellationToken = cancellationToken,
                Tcs = tcs,
            };
            ThreadPool.QueueUserWorkItem(PollAsync, data);
            return tcs.Task;
        }

        private sealed class PollData
        {
            public NfcDevice Initiator { get; internal set; }
            public NfcModulation[] Modulations { get; internal set; }
            public byte Period { get; internal set; }
            public CancellationToken CancellationToken { get; internal set; }
            public TaskCompletionSource<NfcTarget> Tcs { get; internal set; }
        }

        private static void PollAsync(object state)
        {
            PollData data = (PollData)state;
            NfcTarget result = null;
            try
            {
                int i = 0;
                Action cancelPolling = () =>
                {
                    var error = NativeMethods.abort_command(data.Initiator.Handle);
                };
                data.CancellationToken.Register(cancelPolling);
                while (result == null && !data.CancellationToken.IsCancellationRequested)
                {
                    result = ((INfcInitiator)data.Initiator).Poll(new[] { data.Modulations[i] }, 1, data.Period);
                    i = (i + 1) % data.Modulations.Length;
                }
                if (data.CancellationToken.IsCancellationRequested)
                {
                    data.Tcs.TrySetCanceled(data.CancellationToken);
                }
                else
                {
                    data.Tcs.TrySetResult(result);
                }
            }
            catch (Exception ex)
            {
                data.Tcs.TrySetException(ex);
            }
            finally
            {
                data.Tcs.TrySetResult(result);
            }
        }
    }
}
