using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DoggyServer
{
    public class PID
    {
        private static string pidFile = "DoggyServer.pid";

        public static Boolean check(string[] args)
        {
            if (args.Contains("deletepid")) ending();
            if (File.Exists(pidFile)) return (false);

            StreamWriter s = new StreamWriter(pidFile);

            s.Write("is running");

            s.Close();

            return (true);
        }

        public static void ending()
        {
            if (File.Exists(pidFile)) File.Delete(pidFile);
        }
    }
}