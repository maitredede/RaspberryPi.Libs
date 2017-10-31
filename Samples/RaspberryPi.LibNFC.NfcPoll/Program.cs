using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPi.LibNFC.NfcPoll
{
    class Program
    {
        public static void Main(string[] args)
        {
            MainSelfLibNfc(args);
        }

        public static void MainSelfLibNfc(string[] args)
        {
            Console.WriteLine("Hello");
            Console.WriteLine("Using libnfc {0}", NfcContext.Version);

            //CancellationTokenSource cts = new CancellationTokenSource();
            //Console.CancelKeyPress += (s, e) =>
            //{
            //    if (cts.IsCancellationRequested)
            //    {
            //        //Hard break, let it go
            //    }
            //    else
            //    {
            //        //soft break, handle it
            //        e.Cancel = true;
            //        cts.Cancel();
            //    }
            //};

            byte pollNr = 1;
            byte period = 1;
            var modulations = new[]
            {
                new NfcModulation(NfcModulationType.ISO14443A, NfcBaudRate.BR106),
                new NfcModulation(NfcModulationType.ISO14443B, NfcBaudRate.BR106),
                new NfcModulation(NfcModulationType.Felica, NfcBaudRate.BR212),
                new NfcModulation(NfcModulationType.Felica, NfcBaudRate.BR424),
                new NfcModulation(NfcModulationType.Jewel, NfcBaudRate.BR106),
            };
            Console.WriteLine("Opening libnfc context. sizeof(NfcModulation)={0}", Marshal.SizeOf<NfcModulation>());
            using (NfcContext context = new NfcContext())
            {
                string[] devices = context.ListDevices();
                Console.WriteLine("Found devices: [{0}]", string.Join(";", devices));
                Console.WriteLine("Opening default device");
                using (NfcDevice device = context.Open(null))
                {
                    Console.WriteLine("Device opened");
                    Console.WriteLine("  name={0} constring={1}", device.Name, device.ConnectionString);
                    Console.WriteLine("  info={0}", device.GetInfo());
                    Console.WriteLine("Initializing as initiator");
                    INfcInitiator initiator = device.InitInitiator(false);
                    Console.WriteLine("NFC reader opened");
                    Console.WriteLine("NFC reader name: {0}", device.Name);

                    while (true)
                    {
                        using (var tags = device.FreefareGetTags())
                        {
                            if (tags == null)
                            {
                                Console.WriteLine("Tags is null");
                                return;
                            }
                            Console.WriteLine("Found tags : {0}", tags.Count);
                            foreach (var tag in tags)
                            {
                                Console.WriteLine("  {0}: type={1} name={2}", tag.GetType().Name, tag.Type, tag.Name);
                            }
                        }
                    }

                    //Console.WriteLine($"NFC device will poll during {pollNr * modulations.Length * period * 150}ms ({pollNr} pollings of {period * 150}ms for {modulations.Length} modulations)");
                    //NfcTarget target = null;
                    //do
                    //{
                    //    target = initiator.Poll(modulations, pollNr, period);
                    //}
                    //while (target == null);
                    //NfcInitiatorTarget target = await initiator.PollAsync(modulations, period, cts.Token);
                    //Console.WriteLine("Found receiver device");

                    ////TODO card handling read/write/info

                    //while (target.IsPresent())
                    //{
                    //    Thread.Sleep(10);
                    //}

                    //Console.WriteLine("Receiver device removed");
                }
            }
        }
    }
}
