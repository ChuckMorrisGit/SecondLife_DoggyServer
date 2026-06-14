using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyMCP.Net_Sub
{
    class Message_Fire
    {
        private static List<DoggyMCP.SL_Client_Sub.SL_Message> message_fire_list = new List<SL_Client_Sub.SL_Message>();
        private static System.Threading.Thread message_fire_thread;
        private static Boolean thread_running = false;

        public static void to_all(DoggyMCP.SL_Client_Sub.SL_Message message)
        {
            message_fire_list.Add(message);

            if (!thread_running)
            {
                thread_running = true;
                message_fire_thread = new System.Threading.Thread(new System.Threading.ThreadStart(message_fire_thread_run));
                message_fire_thread.IsBackground = true;
                message_fire_thread.Start();
            }
        }

        private static void message_fire_thread_run()
        {
            while (message_fire_list.Count > 0)
            {
                DoggyMCP.SL_Client_Sub.SL_Message message = message_fire_list[0];
                try
                {
                    Net_Sub.SimpleCallback.FireNewBroadcastedMessageEvent(message);
                }
                catch (Exception ex) { Output.WriteLine("MESSAGE ERROR: " + ex.Message + " -> " + message.message); }

                message_fire_list.Remove(message);
            }

            thread_running = false;
        }
    }
}
