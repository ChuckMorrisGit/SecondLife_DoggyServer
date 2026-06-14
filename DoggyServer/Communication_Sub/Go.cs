using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Go
    {
        public static void to(GridClient client, string[] commandArray, UUID fromAV, string fromName)
        {
            try
            {
                string fullParam = string.Empty;
                int i = 2;
                while (i < commandArray.Count())
                {
                    fullParam += commandArray[i] + " ";
                    i++;
                }
                fullParam = fullParam.TrimEnd(' ');

                switch (commandArray[1])
                {
                    case "home":
                        DoggyServer_main.scanMode = false;
                        Teleport_Sub.Home.go(client);
                        break;

                    default:
                        Movement_Sub.Target.Parse.target(client, commandArray[1], fullParam, fromAV);
                        break;
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("GO TARGET ERROR: " + ex.Message, false); }
        }

    }
}
