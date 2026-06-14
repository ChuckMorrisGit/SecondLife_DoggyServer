using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;

namespace DoggyServer.ViewerLogFile_Sub
{
    class init
    {
        private static string[] lines;
        private static int linepos = 0;
        private static Boolean run = false;

        public static void initall(GridClient client)
        {
            run = true;
        }

        public static void TimerTask()
        {
            if (run)
            {
                try
                {
                    lines = File.ReadAllLines(@"C:\Users\[WINDOWS_USER]\AppData\Roaming\SecondLife\logs\PhoenixViewer.log");

                    string line;
                    while (linepos < lines.Count())
                    {
                        line = lines[linepos];

                        Output_sub.Logs.add("LOCAL ID: " + line, true);
                        if (line.ToLower().Contains("LLViewerObject::dump: LocalID".ToLower()))
                        {
                            string[] lineArray = line.Split(' ');

                            Output_sub.Logs.add("LOCAL ID: " + lineArray[lineArray.Count() - 1], true);
                        }

                        linepos++;
                    }
                }
                catch (Exception ex) { Output_sub.Logs.add("VIEWER LOG: " + ex.Message, false); }
            }
        }
    }
}