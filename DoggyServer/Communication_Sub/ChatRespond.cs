using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class ChatRespond
    {
        private static GridClient client;
        private static List<string> responds = new List<string>();
        private static List<string> catresponds = new List<string>();
        private static Random rand = new Random();
        private static Dictionary<string, DateTime> chatHistory = new Dictionary<string, DateTime>();

        private static string[] koeterTexte =
        {
            "Köter? Please change your language. To a human language",
            "I peed @ your pants",
            "And noone likes you",
            "Hear your mom is yelling",
            "you'r smelling"
        };

        private static string[] mystyTexte =
        {
            "may I use your leg, Mysty?",
            "You smelling so sweet Mysty",
            "I ♥ U",
            "I wanna be you pet, Mysty",
            "I love your style, Mysty",
            "in my next life I want to be a Cat to do some new with you, Mysty",
            "I have an eye on you. You belong to Walter, Mysty"
        };

        public static void init(GridClient client_new)
        {
            client = client_new;
            responds.Add("*wuff*");
            responds.Add("*hechel*");
            //responds.Add("knurrr");
            responds.Add("*wafff*");

            catresponds.Add("*Miau*");
            catresponds.Add("*Mauz*");
            catresponds.Add("*schnurr*");
        }

        public static void say(GridClient client, ChatEventArgs chatEventArgs)
        {
            string name = chatEventArgs.FromName;
            if (!chatHistory.Keys.Contains(name))
                chatHistory.Add(name, DateTime.Now);

            TimeSpan timeSpan = DateTime.Now - chatHistory[name];

            if ((timeSpan.TotalSeconds > 5) && (name != client.Self.Name))
            {
                try
                {
                    if (DoggyServer_main.agentData.type == AvaData.avaType.doggy)
                    {
                        if ((chatEventArgs.Message.ToLower().Contains("köter")) || (chatEventArgs.Message.ToLower().Contains("koeter")))
                        {
                            if (!name.Contains("Doggy"))
                            {
                                float ran = rand.Next(koeterTexte.Count());
                                client.Self.Chat(koeterTexte[(int)Math.Floor(ran)] + ", " + chatEventArgs.FromName, 0, ChatType.Normal);
                                chatHistory[name] = DateTime.Now;
                            }
                        }

                        if (chatEventArgs.Message.ToLower().Contains("doggy"))
                        {
                            if (name.ToLower() == "someone else")
                            {
                                float ran = rand.Next(mystyTexte.Count());
                                client.Self.Chat(mystyTexte[(int)Math.Floor(ran)], 0, ChatType.Normal);
                            }
                            else
                            {
                                int which = rand.Next(0, responds.Count - 1);
                                client.Self.Chat(responds[which], 0, ChatType.Normal);
                                chatHistory[name] = DateTime.Now;
                            }
                        }
                    }

                    if (DoggyServer_main.agentData.type == AvaData.avaType.catty)
                    {
                        if (chatEventArgs.Message.ToLower().Contains("catty"))
                        {
                            if (name.ToLower() == "someone else")
                            {
                                client.Self.Chat("*schnurr* @ someone else. *Beine umschlengel*", 0, ChatType.Normal);
                            }
                            else
                            {
                                int which = rand.Next(0, catresponds.Count - 1);
                                client.Self.Chat(catresponds[which], 0, ChatType.Normal);
                                chatHistory[name] = DateTime.Now;
                            }
                        }
                    }
                }
                catch (Exception exp) { Output_sub.Logs.add("ERROR: " + exp.Message, false); }
            }
        }
    }
    

    class ChatTime
    {
        public DateTime when = DateTime.Now;
        public string who = string.Empty;
    }
}
