using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Output_sub
{
    class Logs
    {
        public static List<string> logs = new List<string>();
        public static List<string> chats = new List<string>();
        public static int maxLogLength = 10;
        public static int maxchatLength = 10;
        public static GridClient client;

        public static void add(string logText, Boolean ifChat)
        {
            if (ifChat)
            {
                chats.Add(DateTime.Now.ToString() + " " + logText);
                while (chats.Count > maxLogLength) chats.Remove(chats[0]);
            }
            else
            {
                logs.Add(DateTime.Now.ToString() + " " + logText);
                while (logs.Count > maxLogLength) logs.Remove(logs[0]);
                LogFile.write(client, logText);

            }

            MainScreen.Output(client);
        }

        public static void init(GridClient client_new)
        {
            client = client_new;

            OpenMetaverse.Logger.OnLogMessage += new Logger.LogCallback(Logger_OnLogMessage);
            
        }

        public static int errorCounter = 0;
        static void Logger_OnLogMessage(object message, Helpers.LogLevel level)
        {
            add("Logger-LOGS: " + message.ToString(),false);

            //if (level == Helpers.LogLevel.Error) DoggyServer_main.running = false;

            if (message.ToString().Contains("NetworkManager shutdown initiated"))
            {
                Output_sub.Logs.add("ERROR Logger_OnLogMessage: " + "NetworkManager shutdown initiated", false);
                DoggyServer_main.running = false;
            }

            if (message.ToString().Contains("Packet received before simulator packet processing threads running"))
            {
                errorCounter++;
                if (errorCounter > 10)
                {
                    //DoggyServer_main.running = false;
                    //Environment.Exit(1);
                }
            }

        }
    }
}
