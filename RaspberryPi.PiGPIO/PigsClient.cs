using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RaspberryPi.PiGPIO
{
    /// <summary>
    /// PiGpio socket client
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class PigsClient : IPiGPIO, IDisposable
    {
        private const ushort NTFY_FLAGS_EVENT = (1 << 7);
        private const ushort NTFY_FLAGS_ALIVE = (1 << 6);
        private const ushort NTFY_FLAGS_WDOG = (1 << 5);
        private const ushort NTFY_FLAGS_GPIO = 31;

        private readonly string m_host;
        private readonly int m_port;

        private readonly SocketFlux m_control;
        private readonly SocketFlux m_notif;

        private readonly object m_callbackLock;
        private readonly List<CallbackInfo> m_callbacks;
        private Thread m_notificationReader;
        private Thread m_notificationRaiser;
        private bool m_listenCallbacks;
        private BufferBlock<NotificationData> m_notifBuffer;

        private uint m_lastLevel;
        private uint m_monitor;
        private uint m_events;
        private uint m_handle;

        public bool IsConnected => this.m_control.IsConnected;
        public EndPoint EndPoint => this.m_control.EndPoint;

        /// <summary>
        /// Helper for I2C I/O
        /// </summary>
        public I2CHelper I2C { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PigsClient"/> class.
        /// </summary>
        public PigsClient() : this(IPAddress.Loopback.ToString(), 8888)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PigsClient"/> class.
        /// </summary>
        /// <param name="host">Remote host.</param>
        /// <param name="port">Remote port.</param>
        public PigsClient(string host, int port)
        {
            this.m_host = host;
            this.m_port = port;
            this.I2C = new I2CHelper(this);

            this.m_callbackLock = new object();
            this.m_callbacks = new List<CallbackInfo>();

            this.m_control = new SocketFlux(5000);
            this.m_notif = new SocketFlux(100);
        }

        public void Dispose()
        {
            this.StopThread();
            this.m_notif.Dispose();
            this.m_control.Dispose();
        }

        ///// <summary>
        ///// Connects to the remote host
        ///// </summary>
        ///// <returns></returns>
        //public async Task ConnectAsync()
        //{
        //    await Task.WhenAll(this.m_control.Connect(this.m_host, this.m_port), this.m_notif.Connect(this.m_host, this.m_port));

        //    this.m_lastLevel = BR1(this.m_notif);
        //    this.m_handle = NOIB(this.m_notif);
        //    this.NB(this.m_handle, this.m_monitor);
        //    this.StartThread();
        //}

        private Task m_connectTask;

        /// <summary>
        /// Connects to the remote host
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            lock (this)
            {
                if (this.m_connectTask == null)
                {
                    this.m_connectTask = Task.WhenAll(this.m_control.Connect(this.m_host, this.m_port), this.m_notif.Connect(this.m_host, this.m_port));
                }
            }
            await this.m_connectTask;

            this.m_lastLevel = BR1(this.m_notif);
            this.m_handle = NOIB(this.m_notif);
            this.NB(this.m_handle, this.m_monitor);
            this.StartThread();

            lock (this)
            {
                this.m_connectTask = null;
            }
        }

        private void StopThread()
        {
            Thread thRead;
            Thread thNotif;
            BufferBlock<NotificationData> buffer;
            lock (this.m_callbackLock)
            {
                thRead = this.m_notificationReader;
                thNotif = this.m_notificationRaiser;
                buffer = this.m_notifBuffer;
                this.m_listenCallbacks = false;
            }
            if (thRead != null)
            {
                thRead.Join();
                this.m_notificationReader = null;
                buffer.Complete();
                thNotif.Join();
                this.m_notificationRaiser = null;
                this.m_notifBuffer = null;
            }
        }

        private void StartThread()
        {
            lock (this.m_callbackLock)
            {
                if (this.m_notificationReader == null)
                {
                    this.m_listenCallbacks = true;
                    this.m_notifBuffer = new BufferBlock<NotificationData>(new DataflowBlockOptions() { EnsureOrdered = true });

                    this.m_notificationReader = new Thread(this.RunCallbackRead);
                    this.m_notificationReader.Name = "PigsEvents_listener";

                    this.m_notificationRaiser = new Thread(this.RunCallbackRaise);
                    this.m_notificationRaiser.Name = "PigsEvents_raise";

                    this.m_notificationReader.Start();
                    this.m_notificationRaiser.Start();
                }
            }
        }


        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#M/MODES
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="mode"></param>
        public void SetMode(int gpio, Mode mode)
        {
            Commands cmd = Commands.MODES;
            uint p1 = (uint)gpio;
            uint p2 = (uint)mode;
            int p3 = 0;
            byte[] extension = null;
            int res;
            byte[] resExtension;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res, out resExtension);
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#MG/MODEG
        /// </summary>
        /// <param name="gpio"></param>
        /// <returns></returns>
        public Mode GetMode(int gpio)
        {
            Commands cmd = Commands.MODEG;
            uint p1 = (uint)gpio;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = null;
            int res;
            byte[] resExtension;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res, out resExtension);

            return (Mode)res;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#PUD
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="pullUp"></param>
        public void SetPullUpDown(int gpio, PullUpDown pullUp)
        {
            Commands cmd = Commands.PUD;
            uint p1 = (uint)gpio;
            uint p2 = (uint)pullUp;
            int p3 = 0;
            byte[] extension = null;
            int res;
            byte[] resExtension;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res, out resExtension);
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#R/READ
        /// </summary>
        /// <param name="gpio"></param>
        /// <returns></returns>
        public bool Read(int gpio)
        {
            Commands cmd = Commands.READ;
            uint p1 = (uint)gpio;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = null;
            int res;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);

            return res != 0;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#W/WRITE
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="value"></param>
        public void Write(int gpio, bool value)
        {
            Commands cmd = Commands.WRITE;
            uint p1 = (uint)gpio;
            uint p2 = value ? 1u : 0;
            int p3 = 0;
            byte[] extension = null;
            int res;
            byte[] resExtension;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res, out resExtension);
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#P/PWM
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="dutycycle"></param>
        /// <returns></returns>
        public int PWM(int gpio, byte dutycycle)
        {
            Commands cmd = Commands.PWM;
            uint p1 = (uint)gpio;
            uint p2 = dutycycle;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            return res;
        }

        //TODO PRS
        //TODO PFS
        //TODO SERVO
        //TODO WDOG

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#BR1
        /// Read bank 1
        /// </summary>
        /// <returns></returns>
        public int BR1()
        {
            return unchecked((int)BR1(this.m_control));
        }

        private static uint BR1(SocketFlux socket)
        {
            Commands cmd = Commands.BR1;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            uint res;
            socket.RunCommand(cmd, p1, p2, p3, extension, out res);
            return res;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pdif2.html#spi_open
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="bauds"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public int SpiOpen(int channel, int bauds, int flags)
        {
            return this.SpiOpen((uint)channel, (uint)bauds, (uint)flags);
        }
        internal int SpiOpen(uint channel, uint bauds, uint flags)
        {
            Commands cmd = Commands.SPIO;
            uint p1 = channel;
            uint p2 = bauds;
            byte[] extension = BitConverter.GetBytes(flags);
            int p3 = extension.Length;
            int res;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);

            return res;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#SPIC
        /// </summary>
        /// <param name="handle"></param>
        public void SpiClose(int handle)
        {
            Commands cmd = Commands.SPIC;
            uint p1 = (uint)handle;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = null;
            int res;
            byte[] resExtension;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res, out resExtension);
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#SPIR
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] SpiRead(int handle, int count)
        {
            return SpiRead(handle, (uint)count);
        }

        internal byte[] SpiRead(int handle, uint count)
        {
            Commands cmd = Commands.SPIR;
            uint p1 = (uint)handle;
            uint p2 = count;
            int p3 = 0;
            byte[] extension = null;

            int res;
            byte[] resExtension;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res, out resExtension);

            return resExtension;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#SPIW
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="data"></param>
        public void SpiWrite(int handle, byte[] data)
        {
            Commands cmd = Commands.SPIW;
            uint p1 = (uint)handle;
            uint p2 = 0;
            int p3 = data.Length;
            byte[] extension = data;
            int res;

            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#HWVER
        /// </summary>
        /// <returns></returns>
        public int HardwareRevision()
        {
            Commands cmd = Commands.HWVER;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            return res;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#PIGPV
        /// </summary>
        /// <returns></returns>
        public int PigpioVersion()
        {
            Commands cmd = Commands.PIGPV;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            return res;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#PIGPV
        /// </summary>
        /// <returns></returns>
        public int PIGPV()
        {
            Commands cmd = Commands.PIGPV;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            return res;
        }

        /// <summary>
        /// Creates a waveform from the data provided by the prior calls to the WVAG and WVAS commands.
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#WVCRE
        /// </summary>
        /// <returns>Upon success a wave id (>=0) is returned. On error a negative status code will be returned</returns>
        public int WaveformCreate()
        {
            Commands cmd = Commands.WVCRE;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            return res;
        }

        /// <summary>
        /// This clears any existing waveform data ready for the creation of a new waveform.
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#WVNEW
        /// </summary>
        public void WaveFormNew()
        {
            Commands cmd = Commands.WVNEW;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            //return res;
        }

        /// <summary>
        /// NOIB only works on the socket interface. It returns a spare notification handle.Notifications for that handle will be sent to the socket. The socket should be dedicated to receiving notifications after this command is issued
        /// </summary>
        /// https://github.com/joan2937/pigpio/blob/master/pigpio.h#L6114
        /// <returns>A spare notification handle</returns>
        public int NOIB()
        {
            return unchecked((int)NOIB(this.m_control));
        }

        private static uint NOIB(SocketFlux socket)
        {
            Commands cmd = Commands.NOIB;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            socket.RunCommand(cmd, p1, p2, p3, extension, out res);
            return unchecked((uint)res);
        }

        public void NB(int handle, int bits)
        {
            this.NB(unchecked((uint)handle), unchecked((uint)bits));
        }

        internal void NB(uint handle, uint bits)
        {
            Commands cmd = Commands.NB;
            uint p1 = handle;
            uint p2 = bits;
            int p3 = 0;
            byte[] extension = new byte[0];
            uint res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
        }

        public void NC(int handle)
        {
            NC(this.m_control, unchecked((uint)handle));
        }

        private static void NC(SocketFlux socket, uint handle)
        {
            Commands cmd = Commands.NC;
            uint p1 = handle;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            uint res;
            socket.RunCommand(cmd, p1, p2, p3, extension, out res);
        }

        /// <summary>
        /// Set a noise filter on a GPIO
        /// Level changes on the GPIO <paramref name="gpio"/> are ignored until a level which has been stable for <paramref name="stdy"/> microseconds is detected. Level changes on the GPIO are then reported for <paramref name="actv"/> microseconds after which the process repeats.
        /// The filter only affects callbacks (including pipe notifications).
        /// The R/READ, BR1, and BR2 commands are not affected.
        /// Note, level changes before and after the active period may be reported.Your software must be designed to cope with such reports.
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="stdy"></param>
        /// <param name="actv"></param>
        public void NoiseFilter(int gpio, int stdy, int actv)
        {
            Commands cmd = Commands.FN;
            uint p1 = (uint)gpio;
            uint p2 = (uint)stdy;
            int p3 = 4;
            byte[] extension = BitConverter.GetBytes(actv);
            uint res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
        }

        private byte[] GetBytes<T>(T str)
        {
            int size = Marshal.SizeOf<T>();
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        private byte[] GetBytes<T>(T[] str)
        {
            int singleSize = Marshal.SizeOf<T>();
            int size = singleSize * str.Length;
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            IntPtr pos = ptr;
            for (int i = 0; i < str.Length; i++)
            {
                Marshal.StructureToPtr(str[i], pos, true);
                pos = pos + singleSize;
            }

            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        //[Obsolete("Prefer the overload with structured data")]
        //public void WVAG(uint on, uint off, uint usDelay)
        //{
        //    //gpioPulse_t pulse = new gpioPulse_t(on, off, delayUS);
        //    //byte[] data = this.GetBytes<gpioPulse_t>(pulse);

        //    //uint cmd = 28;
        //    //uint p1 = 0;
        //    //uint p2 = 0;
        //    //int p3 = data.Length;
        //    //byte[] extension = data;
        //    //int res;
        //    //this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
        //    ////return res;
        //    this.WVAG(new Pulse[] { new Pulse(on, off, usDelay) });
        //}

        //public void WVAG(gpioPulse_t[] pulseData)
        //{
        //    byte[] data = this.GetBytes(pulseData);

        //    Commands cmd = Commands.WVAG;
        //    uint p1 = 0;
        //    uint p2 = 0;
        //    int p3 = data.Length;
        //    byte[] extension = data;
        //    int res;
        //    this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
        //}

        public void WaveformAppend(Pulse[] pulseData)
        {
            byte[] data = this.GetBytes(pulseData);

            Commands cmd = Commands.WVAG;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = data.Length;
            byte[] extension = data;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
        }

        public void WaveformDelete(int wid)
        {
            Commands cmd = Commands.WVDEL;
            uint p1 = (uint)wid;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            //return res;
        }

        public int WVTX(uint wid)
        {
            Commands cmd = Commands.WVTX;
            uint p1 = wid;
            uint p2 = 0;
            int p3 = 0;
            byte[] extension = new byte[0];
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, extension, out res);
            return res;
        }

        /// <summary>
        /// Execute a shell command.
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#SHELL
        /// </summary>
        /// <param name="name">The shell script. Must exist in /opt/pigpio/cgi and must be executable. </param>
        /// <param name="args">The arguments.</param>
        /// <returns>Returns the shell script return value mulitplied by 256, or 32512 if script is not found</returns>
        public int SHELL(string name, string args)
        {
            byte[] bName = Encoding.UTF8.GetBytes(name);
            byte[] bArgs = Encoding.UTF8.GetBytes(args);
            byte[] data = new byte[bName.Length + bArgs.Length + 1];
            Array.Copy(bName, data, bName.Length);
            Array.Copy(bArgs, 0, data, bName.Length + 1, bArgs.Length);

            Commands cmd = Commands.SHELL;
            uint p1 = (uint)bName.Length;
            uint p2 = 0;
            int p3 = data.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res);
            return res;
        }

        public int FileOpen(string file, int mode)
        {
            byte[] data = Encoding.UTF8.GetBytes(file);

            Commands cmd = Commands.FO;
            uint p1 = (uint)mode;
            uint p2 = 0;
            int p3 = data.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res);
            return res;
        }

        public void FileClose(int handle)
        {
            Commands cmd = Commands.FC;
            uint p1 = (uint)handle;
            uint p2 = 0;
            byte[] data = new byte[0];
            int p3 = data.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res);
            //return res;
        }

        /// <summary>
        /// http://abyz.co.uk/rpi/pigpio/pigs.html#FR
        /// Read bytes from file handle
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] FileRead(int handle, int count)
        {
            Commands cmd = Commands.FR;
            uint p1 = (uint)handle;
            uint p2 = (uint)count;
            byte[] data = new byte[0];
            int p3 = data.Length;
            int res;
            byte[] result;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res, out result);

            return result;
        }

        public void FileWrite(int handle, byte[] data)
        {
            Commands cmd = Commands.FW;
            uint p1 = (uint)handle;
            uint p2 = 0;
            int p3 = data.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res);
        }

        public int FileSeek(int handle, int offset, int from)
        {
            byte[] data = BitConverter.GetBytes(from);

            Commands cmd = Commands.FS;
            uint p1 = (uint)handle;
            uint p2 = (uint)offset;
            int p3 = data.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res);
            return res;
        }

        public CallbackInfo AddCallback(int userGpio, Edge edge, GpioCallback callback)
        {
            CallbackInfo info;
            uint bits = 1u << userGpio;
            lock (this.m_callbacks)
            {
                uint newMonitor = this.m_monitor | bits;
                if (this.IsConnected)
                {
                    this.NB(this.m_handle, newMonitor);
                }
                this.m_monitor = newMonitor;
                info = new CallbackInfo((uint)userGpio, bits, edge, callback);
                this.m_callbacks.Add(info);
            }
            return info;
        }

        public void RemoveCallback(CallbackInfo callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            lock (this.m_callbackLock)
            {
                if (!this.m_callbacks.Contains(callback))
                    return;

                uint newMonitor = 0;
                this.m_callbacks.ForEach(c => newMonitor |= c.Bits);
                if (this.IsConnected && newMonitor != this.m_monitor)
                {
                    this.NB(this.m_handle, newMonitor);
                }
                this.m_monitor = newMonitor;
            }
        }

        private void RunCallbackRead(object state)
        {
            uint lastLevel = this.m_lastLevel;
            try
            {
                const int MSG_SIZE = 12;
                byte[] buffer = new byte[MSG_SIZE];
                int read;
                int totalRead;
                while (this.m_listenCallbacks)
                {
                    totalRead = 0;
                    while (this.m_listenCallbacks && totalRead < MSG_SIZE)
                    {
                        SocketError err;
                        read = this.m_notif.Receive(buffer, totalRead, MSG_SIZE - totalRead, SocketFlags.None, out err);
                        totalRead += read;
                        switch (err)
                        {
                            case SocketError.TimedOut:
                                continue;
                            case SocketError.Success:
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    if (totalRead < MSG_SIZE)
                        continue;

                    ////seq, flags, tick, level = (struct.unpack('HHII', buf))
                    //ushort seq = this.m_sck.Reader.ReadUInt16();
                    //ushort flags = this.m_sck.Reader.ReadUInt16();
                    //uint tick = this.m_sck.Reader.ReadUInt32();
                    //uint level = this.m_sck.Reader.ReadUInt32();
                    ushort seq = BitConverter.ToUInt16(buffer, 0);
                    ushort flags = BitConverter.ToUInt16(buffer, 2);
                    uint tick = BitConverter.ToUInt32(buffer, 4);
                    uint level = BitConverter.ToUInt32(buffer, 8);

                    if (!this.m_listenCallbacks)
                        continue;
                    uint changed = level ^ lastLevel;
                    lastLevel = level;
                    this.m_notifBuffer.Post(new NotificationData(seq, flags, tick, level, changed));
                }
                NC(this.m_notif, this.m_handle);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void RunCallbackRaise(object state)
        {
            while (this.m_notifBuffer.OutputAvailableAsync().Result)
            {
                NotificationData data = this.m_notifBuffer.Receive();
                if (data.Flags == 0)
                {
                    CallbackInfo[] callbacks;
                    lock (this.m_callbackLock)
                    {
                        callbacks = this.m_callbacks.ToArray();
                    }
                    foreach (CallbackInfo cb in callbacks)
                    {
                        if ((cb.Bits & data.Changed) != 0)
                        {
                            if ((cb.Bits & data.Level) == 0)
                            {
                                if (cb.Edge == Edge.Fall || cb.Edge == Edge.Either)
                                {
                                    cb.Callback(cb.Port, Edge.Fall, data.Tick);
                                }
                            }
                            else
                            {
                                if (cb.Edge == Edge.Rise || cb.Edge == Edge.Either)
                                {
                                    cb.Callback(cb.Port, Edge.Rise, data.Tick);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((data.Flags & NTFY_FLAGS_WDOG) != 0)
                    {
                        ushort gpio = (ushort)(data.Flags & NTFY_FLAGS_GPIO);
                        //   for cb in self.callbacks:
                        //      if cb.gpio == gpio:
                        //         cb.func(gpio, TIMEOUT, tick)
                    }
                    else
                    {
                        if ((data.Flags & NTFY_FLAGS_EVENT) != 0)
                        {
                            ushort evt = (ushort)(data.Flags & NTFY_FLAGS_GPIO);
                            //   for cb in self.events:
                            //      if cb.event == event:
                            //         cb.func(event, tick)
                        }
                    }
                }
            }
        }


        public int I2COpen(int bus, int address, int flags)
        {
            byte[] data = BitConverter.GetBytes(flags);

            Commands cmd = Commands.I2CO;
            uint p1 = (uint)bus;
            uint p2 = (uint)address;
            int p3 = data.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res);
            return res;
        }

        public void I2CClose(int handle)
        {
            Commands cmd = Commands.I2CC;
            uint p1 = (uint)handle;
            uint p2 = 0;
            int p3 = 0;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, null, out res);
        }

        byte[] IPiGPIO.I2CReadBytes(int handle, int num)
        {
            throw new NotImplementedException();
        }

        public byte[] SpiXfer(int handle, byte[] txBuffer)
        {
            Commands cmd = Commands.SPIX;
            uint p1 = (uint)handle;
            uint p2 = 0;
            int p3 = txBuffer.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, txBuffer, out res, out byte[] rxBuffer);
            if (res < 0)
                throw new PiGPIOException(res);
            return rxBuffer;
        }

        public void BSpiOpen(int gpioCS, int gpioMiso, int gpioMosi, int gpioClk, int bauds, int flags)
        {
            byte[] data = new byte[20];
            data.Set(0, gpioMiso);
            data.Set(4, gpioMosi);
            data.Set(8, gpioClk);
            data.Set(12, bauds);
            data.Set(16, flags);

            Commands cmd = Commands.BSPIO;
            uint p1 = (uint)gpioCS;
            uint p2 = 0;
            int p3 = data.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, data, out res);
            if (res < 0)
                throw new PiGPIOException(res);
        }

        public void BSpiClose(int gpioCS)
        {
            Commands cmd = Commands.BSPIC;
            uint p1 = (uint)gpioCS;
            uint p2 = 0;
            int p3 = 0;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, null, out res);
            if (res < 0)
                throw new PiGPIOException(res);
        }

        public byte[] BSpiXfer(int gpioCS, byte[] txBuffer)
        {
            Commands cmd = Commands.BSPIX;
            uint p1 = (uint)gpioCS;
            uint p2 = 0;
            int p3 = txBuffer.Length;
            int res;
            this.m_control.RunCommand(cmd, p1, p2, p3, txBuffer, out res, out byte[] rxBuffer);
            if (res < 0)
                throw new PiGPIOException(res);
            return rxBuffer;
        }

        public int ReadBits_0_31()
        {
            Commands cmd = Commands.BR1;
            uint p1 = 0;
            uint p2 = 0;
            int p3 = 0;
            this.m_control.RunCommand(cmd, p1, p2, p3, null, out int res);
            return res;
        }

        public int ReadBits_32_53()
        {
            throw new NotImplementedException();
        }

        public void SetBits_0_31(int bits)
        {
            throw new NotImplementedException();
        }

        public void SetBits_32_53(int bits)
        {
            throw new NotImplementedException();
        }

        public void ClearBits_0_31(int bits)
        {
            throw new NotImplementedException();
        }

        public void ClearBits_32_53(int bits)
        {
            throw new NotImplementedException();
        }
    }
}
