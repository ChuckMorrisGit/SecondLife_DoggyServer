using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace DoggyClient.Comunikation_Sub
{
    class Messages
    {
        public static Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>> chats = new Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>>();
        public static Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>> IMs = new Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>>();
        public static Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>> G_IMs = new Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>>();
        public static Dictionary<UUID, DoggyMCP.Comunication_Sub.Group_data> groups = new Dictionary<UUID, DoggyMCP.Comunication_Sub.Group_data>();

        public static void get_history(UUID ava_key)
        {
            if (!chats.ContainsKey(ava_key)) chats.Add(ava_key, new List<DoggyMCP.Comunication_Sub.Com_data>());
            if (!IMs.ContainsKey(ava_key)) IMs.Add(ava_key, new List<DoggyMCP.Comunication_Sub.Com_data>());
            if (!G_IMs.ContainsKey(ava_key)) G_IMs.Add(ava_key, new List<DoggyMCP.Comunication_Sub.Com_data>());

            try
            {
                List<DoggyMCP.Comunication_Sub.Group_data> group_list = MCP_Sub.init.service.get_groups();
                foreach (DoggyMCP.Comunication_Sub.Group_data group in group_list)
                {
                    if (!groups.ContainsKey(group.group_id)) groups.Add(group.group_id, group);
                }
            }
            catch (Exception ex)
            {
                MCP_Sub.init.RegisterService();
            }

            try
            {
                chats[ava_key] = MCP_Sub.init.service.get_chat(ava_key);
            }
            catch (Exception ex)
            {
                MCP_Sub.init.RegisterService();
            }

            try
            {
                IMs[ava_key] = MCP_Sub.init.service.get_im(ava_key);
            }
            catch (Exception ex)
            {
                MCP_Sub.init.RegisterService();
            }

            try
            {
                G_IMs[ava_key] = MCP_Sub.init.service.get_g_im(ava_key);
            }
            catch (Exception ex)
            {
                MCP_Sub.init.RegisterService();
            }
        }

        public static void say_chat(UUID ava_key, string line)
        {

            try
            {
                Boolean isnu = MCP_Sub.init.service.set_say_chat(ava_key, line);
            }
            catch (Exception ex)
            {
                MCP_Sub.init.RegisterService();
            }
        }

        public static void say_session_id(UUID ava_key, UUID session_id, string line)
        {

            try
            {
                Boolean isnu = MCP_Sub.init.service.set_say_IM(ava_key, session_id, line);
            }
            catch (Exception ex)
            {
                MCP_Sub.init.RegisterService();
            }

        }

        public static void add_Message_Line(DoggyMCP.SL_Client_Sub.SL_Message message)
        {
            DoggyMCP.Comunication_Sub.Com_data com_data = new DoggyMCP.Comunication_Sub.Com_data();
            com_data.line = message.message;
            com_data.session_id = message.session_id;
            com_data.timestamp = DateTime.Now;

            switch (message.message_art)
            {
                case DoggyMCP.SL_Client_Sub.SL_Message.Message_art.chat:
                    if (chats.ContainsKey(message.agent_id)) chats[message.agent_id].Add(com_data);
                    break;

                case DoggyMCP.SL_Client_Sub.SL_Message.Message_art.im:
                    if (IMs.ContainsKey(message.agent_id)) IMs[message.agent_id].Add(com_data);
                    break;

                case DoggyMCP.SL_Client_Sub.SL_Message.Message_art.g_im:
                    if (G_IMs.ContainsKey(message.agent_id)) G_IMs[message.agent_id].Add(com_data);
                    break;


            }

            MainForm_Sub.Tabs.add_message_line(message);
        }
    }
}
