using System;
using System.Threading;

namespace PiGPIO.Blink
{
    class Program
    {
        static void Main(string[] args)
        {
            //Config
            int gpioNum = 2;
            string piAddress = "192.168.20.22";
            int port = 8888; //Default port
            int msDelay = 500;

            //Create a client
            using (PigsClient client = new PigsClient(piAddress, port))
            {
                //Connect
                client.ConnectAsync().Wait();
                Console.Write("connected");

                //Set port as output
                client.SetMode(gpioNum, Mode.Output);

                //Value to write
                bool value = false;

                //While the user has not pressed Ctrl+C
                bool run = true;
                Console.TreatControlCAsInput = true;
                while (run)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.C && key.Modifiers == ConsoleModifiers.Control)
                        {
                            run = false;
                            Console.TreatControlCAsInput = false;
                        }
                    }
                    //Blink the LED
                    Console.Title = string.Format("Blink [{0}]", value ? "+" : ".");
                    client.Write(gpioNum, value);
                    value = !value;
                    Thread.Sleep(msDelay);
                }
            }
        }
    }
}