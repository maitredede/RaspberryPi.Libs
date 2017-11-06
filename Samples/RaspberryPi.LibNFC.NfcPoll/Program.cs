using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using RaspberryPi.LibNFC.Mifare;

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
                        PollTagsAndDisplayInfo(initiator);
                    }
                }
            }
        }

        private static void PollTagsAndDisplayInfo(INfcInitiator initiator)
        {
            NfcModulation mod = new NfcModulation(NfcModulationType.ISO14443A, NfcBaudRate.BR106);
            using (var tags = initiator.ListPassiveTargets(mod))
            {
                Console.WriteLine("Found tags: {0} for modulation {1}", tags.Count, mod);
                Console.WriteLine("Last error: {0}", ((NfcDevice)initiator).LastError);
                if (tags.Count == 0)
                    return;

                foreach (var target in tags)
                {
                    DisplayInfo(target);
                }
            }
            ////There are tags, select one
            //Console.WriteLine("Selecting Mifare tag");
            //var tag = initiator.SelectMifareTag();
            //if (tag == null)
            //{
            //    Console.WriteLine("No corresponding tags selected");
            //    return;
            //}
            //using (tag)
            //{
            //    Console.WriteLine("Tag on board");


            //    while (tag.IsPresent())
            //    {
            //        Thread.Sleep(300);
            //        Console.Write(".");
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine("Tag removed");
            //}
        }

        private static void DisplayInfo(NfcTarget target)
        {
            Console.WriteLine("Target info : {0}", target);
            Console.WriteLine("TasteMifareMini: {0}", target.TasteMifareMini());
            Console.WriteLine("TasteMifareClassic1k: {0}", target.TasteMifareClassic1k());
            Console.WriteLine("TasteMifareClassic4k: {0}", target.TasteMifareClassic4k());
        }
    }
}
