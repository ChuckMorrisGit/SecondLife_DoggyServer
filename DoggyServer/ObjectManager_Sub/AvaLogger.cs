using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;

namespace DoggyServer.ObjectManager_Sub
{
    class AvaLogger
    {
        private static Dictionary<string, DateTime> avas = new Dictionary<string, DateTime>();
        private static List<string> notLogged = new List<string>();


        public static void init(GridClient client)
        {
            notLogged.Add("Walter Ginsburg");

        }

        public static void ckeckNlog(GridClient client, Avatar av)
        {
            try
            {
                if (!notLogged.Contains(av.Name))
                {
                    if (!avas.Keys.Contains(av.Name))
                    {
                        avas.Add(av.Name, DateTime.Now);
                        FileStream greeterlogStream = new FileStream("./" + client.Self.LastName + "_SimLogger.log", FileMode.Append);
                        StreamWriter greeterLogFile = new StreamWriter(greeterlogStream);

                        greeterLogFile.Write(DateTime.Now.ToString() + " " + av.Name);
                        greeterLogFile.WriteLine(" -> " + Simulator_Sub.ParcelUpdate.simName + " -> " + Simulator_Sub.ParcelUpdate.parcelName);

                        greeterLogFile.Close();
                    }
                    else
                    {
                        TimeSpan timeSpan = DateTime.Now - avas[av.Name];
                        if (timeSpan.TotalHours > 3) avas.Remove(av.Name);
                    }
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("Ava Check: " + ex.Message, false); }
        }
    }
}
