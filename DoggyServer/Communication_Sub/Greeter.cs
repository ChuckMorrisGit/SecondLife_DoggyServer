using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;

namespace DoggyServer.Communication_Sub
{
    class Greeter
    {
        private static Dictionary<string, DateTime> greetedAvas = new Dictionary<string, DateTime>();
        private static List<string> notToGreetAvas = new List<string>();
        public static Boolean greeterOn = false;

        public static void init()
        {
            greetedAvas.Clear();
            notToGreetAvas.Add("ava name1");
            notToGreetAvas.Add("ava name2");
            notToGreetAvas.Add("ava name3");
            notToGreetAvas.Add("ava name4");
        }

        public static void close()
        {
        }

        public static void checkAndGreet(GridClient client, string avaName, UUID avaKey)
        {
            if ((!DoggyServer_main.scanMode) && (greeterOn))
            {
                try
                {
                    int counter = 0;
                    while (counter < greetedAvas.Count)
                    {
                        TimeSpan timeSpan = DateTime.Now - greetedAvas.Values.First();
                        string avaString = greetedAvas.Keys.First();

                        if (timeSpan.TotalHours > 5)
                        {
                            greetedAvas.Remove(avaString);
                        }
                        else
                        {
                            counter++;
                        }
                    }
                }
                catch (Exception ex) { Output_sub.Logs.add(ex.Message, false); }

                if (!notToGreetAvas.Contains(avaName))
                {
                    if (!greetedAvas.Keys.Contains(avaName))
                    {
                        greetedAvas.Add(avaName, DateTime.Now);
                        switch (client.Self.Name)
                        {
                            case "Ava1":
                                client.Self.Chat("*GRÖÖÖÖÖÖÖÖÖÖÖHHL* @ " + avaName, 0, ChatType.Normal);
                                //if (!Group_Sub.GroupMembers.membersInGroup.Contains(avaKey)) Group_Sub.Invide.byKey(client,avaKey);
                                break;

                            case "Ava2":
                                client.Self.Chat("*Miau Miau* @ " + avaName, 0, ChatType.Normal);
                                break;

                            case "Ava3":
                                client.Self.Chat("*whieher* @ " + avaName, 0, ChatType.Normal);
                                break;

                            default:
                                client.Self.Chat("*hechel* @ " + avaName, 0, ChatType.Normal);
                                break;
                        }

                        FileStream greeterlogStream = new FileStream("./" + client.Self.LastName + "_Greeter.log", FileMode.Append);
                        StreamWriter greeterLogFile = new StreamWriter(greeterlogStream);

                        greeterLogFile.Write(DateTime.Now.ToString() + " " + avaName);
                        greeterLogFile.WriteLine(" -> " + Simulator_Sub.ParcelUpdate.simName + " -> " + Simulator_Sub.ParcelUpdate.parcelName);

                        greeterLogFile.Close();
                    }
                }
            }
        }
    }
}