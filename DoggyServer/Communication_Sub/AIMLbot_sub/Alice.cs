using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIMLbot;

namespace DoggyServer.Communication_Sub.AIMLbot_sub
{
    class Alice
    {
        private static AIMLbot.Bot alice = new Bot();
        public static Hashtable aliceUsers = new Hashtable();

        public static void init()
        {
            alice = new AIMLbot.Bot();
            try
            {
                
                alice.isAcceptingUserInput = false;
                alice.loadSettings(alice.PathToConfigFiles + "/aiml_config/Settings.xml");
                AIMLbot.Utils.AIMLLoader loader = new AIMLbot.Utils.AIMLLoader(alice);
                alice.isAcceptingUserInput = false;
                loader.loadAIML(alice.PathToAIML);
                alice.isAcceptingUserInput = true;

            }
            catch (Exception ex) { Output_sub.Logs.add("ALICE INIT ERROR: " + ex.Message + "\n" + ex.ToString(), false); }

            Output_sub.Logs.add("alice.PathToAIML: " + alice.PathToAIML, false);
            Output_sub.Logs.add("alice.PathToConfigFiles: " + alice.PathToConfigFiles, false);
            Output_sub.Logs.add("alice.PathToLogs: " + alice.PathToLogs, false);

        }

        public static void close()
        {
        }

        public static string response(string text, string fromName)
        {
            string outputText = string.Empty;

            Output_sub.Logs.add("ALICE: " + fromName + " -> " + text, false);

            try
            {
                
                AIMLbot.User user;

                if (aliceUsers.ContainsKey(fromName))
                {
                    user = (AIMLbot.User)aliceUsers[fromName];
                }
                else
                {
                    user = new User(fromName, alice);
                    user.Predicates.removeSetting("name");
                    user.Predicates.addSetting("name", fromName.Split(' ')[0]);
                    aliceUsers[fromName] = user;
                }
                user = new User(fromName.Split(' ')[0], alice);

                AIMLbot.Request req = new Request(text, user, alice);
                AIMLbot.Result res = alice.Chat(req);

                outputText = res.Output;
            }
            catch (Exception ex) { Output_sub.Logs.add("ALICE CHAT ERROR: " + ex.Message, false); }
            return (outputText);
        }
    }
}
