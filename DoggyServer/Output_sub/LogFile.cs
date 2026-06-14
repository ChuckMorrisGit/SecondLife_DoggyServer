using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;

namespace DoggyServer.Output_sub
{
    class LogFile
    {
        //string basedir = Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "character" + System.IO.Path.DirectorySeparatorChar;
        private static string path = "./logs/";
        private static string logfile_name = "_logfile.log";
        private static StreamWriter sw;
        private static string current_logfile;

        public static void init(GridClient client)
        {
            DateTime datetime = DateTime.Now;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Name.Contains(logfile_name))
                {
                    if ((DateTime.Now - fileInfo.CreationTime).TotalDays > 1) File.Delete(file);
                }
            }

            string dateString = string.Format("{0:u}", datetime).Replace(':', '-').Replace(' ', '_'); 
            current_logfile = path + dateString + logfile_name;
            sw = new StreamWriter(current_logfile);
            sw.WriteLine(DateTime.Now.ToString() + ": INIT LOG FILE");
        }

        public static void close(GridClient client)
        {
            sw.Close();
        }

        private static int counter = 0;
        public static void write(GridClient client, string message)
        {
            try
            {
                sw.WriteLine(DateTime.Now.ToString() + ": " + message);

                counter++;
                if (counter > 10)
                {
                    counter = 0;
                    sw.Flush();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
