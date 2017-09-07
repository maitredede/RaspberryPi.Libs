using System;

namespace PiGPIO.Button
{
    class Program
    {
        static void Main(string[] args)
        {
            //Config
            int ledGpio = 2;
            int buttonGpio = 18;
            string piAddress = "192.168.20.11";
            int port = 8888; //Default port

            //Create a client
            using (PigsClient client = new PigsClient(piAddress, port))
            {
                //Connect
                client.ConnectAsync().Wait();
                Console.WriteLine("connected");

                //Set ports
                client.SetMode(ledGpio, Mode.Output);
                client.SetMode(buttonGpio, Mode.Input);
                client.SetPullUpDown(buttonGpio, PullUpDown.Up);

                Console.WriteLine("ready");
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
                    var value = client.Read(buttonGpio);
                    client.Write(ledGpio, !value);
                }
            }
        }
    }
}