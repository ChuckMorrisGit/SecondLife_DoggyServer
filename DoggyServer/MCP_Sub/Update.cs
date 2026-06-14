using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.MCP_Sub
{
    class Update
    {
        public static void run(GridClient client)
        {
            if (init.use_remoting)
            {
                DoggyMCP.SL_Client_Sub.SL_Client sl_Client = new DoggyMCP.SL_Client_Sub.SL_Client();
                sl_Client.id = client.Self.AgentID;
                sl_Client.name = client.Self.Name;

                sl_Client.alice_chat = Communication_Sub.Chat.alice_bot;
                sl_Client.alice_im = Communication_Sub.IM.alice_bot_im;
                sl_Client.alice_g_im = Communication_Sub.IM.alice_bot_groupim;

                sl_Client.online = client.Network.Connected;

                sl_Client.current_master = Master.currentMaster;
                sl_Client.follow = ObjectManager_Sub.ObjectUpdate.followOn;

                sl_Client.chat_follow = Communication_Sub.Chat.chat_follow;
                try
                {
                    Output_sub.Logs.add("UPDATE MCP", false);
                    MCP_Sub.init.service.set_SL_Client(sl_Client, false);
                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("UPDATE MCP ERROR: " + ex.Message, false);
                    Console.WriteLine("\n" + ex.Message + "\n");
                    init.RegisterService();
                }

            }
        }

        public static void get(GridClient client)
        {
            if (init.use_remoting)
            {
                try
                {
                    DoggyMCP.SL_Client_Sub.SL_Client sl_Client = MCP_Sub.init.service.get_SL_Client(client.Self.AgentID);

                    Communication_Sub.Chat.alice_bot = sl_Client.alice_chat;
                    Communication_Sub.IM.alice_bot_im = sl_Client.alice_im;
                    Communication_Sub.IM.alice_bot_groupim = sl_Client.alice_g_im;

                    Master.currentMaster = sl_Client.current_master;
                    ObjectManager_Sub.ObjectUpdate.followOn = sl_Client.follow;

                    Communication_Sub.Chat.chat_follow = sl_Client.chat_follow;

                    Output_sub.Logs.add("UPDATE SERVER FROM MCP", false);

                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("UPDATE MCP ERROR: " + ex.Message, false);
                    Console.WriteLine("\n" + ex.Message + "\n");
                    init.RegisterService();
                }

            }
        }
    }
}
