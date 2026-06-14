using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenMetaverse;


namespace DoggyMCP
{
    class DoggyMCP_main
    {
        public static Boolean verbose = true;
        

        static void Main(string[] args)
        {
            if (verbose)
            {
                //Console.Clear();
                Output.WriteLine("Started @ " + DateTime.Now.ToString());
            }

            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            Net_Sub.Remote.init();

            Output.WriteLine("Please enter to stop the server");
            Console.ReadLine();

            Net_Sub.Remote.close();

            Environment.ExitCode = 1;
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Environment.ExitCode = 0;
            Environment.Exit(0);
        }
    }


}
