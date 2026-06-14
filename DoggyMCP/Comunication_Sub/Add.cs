using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyMCP.Comunication_Sub
{
    class Add
    {
        public static void chat(UUID ava_key, UUID session_id, string line)
        {
            if ( !Data.chats.ContainsKey(ava_key)) Data.chats.Add(ava_key, new List<Com_data>());

            Com_data com_data = new Com_data();
            com_data.session_id = session_id;
            com_data.line = line;
            
            Data.chats[ava_key].Add(com_data);

            if (DoggyMCP_main.verbose)
            {
                Output.Write("Chat for ");
                if (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key)) Output.Write(DoggyMCP.SL_Client_Sub.Data.names[ava_key].name + " -> ");
                else Output.Write(ava_key.ToString() + " -> ");
                Output.WriteLine("\n  " + com_data.line);
            }

            DoggyMCP.SL_Client_Sub.SL_Message message = MCP_Object_Generate(ava_key, session_id, line);
            message.message_art = SL_Client_Sub.SL_Message.Message_art.chat;
            Net_Sub.Message_Fire.to_all(message);

        }

        public static void im(UUID ava_key, UUID session_id, string line)
        {
            if (!Data.IMs.ContainsKey(ava_key)) Data.IMs.Add(ava_key, new List<Com_data>());

            Com_data com_data = new Com_data();
            com_data.session_id = session_id;
            com_data.line = line;

            Data.IMs[ava_key].Add(com_data);

            if (DoggyMCP_main.verbose)
            {
                Output.Write("IM for ");
                if (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key)) Output.Write(DoggyMCP.SL_Client_Sub.Data.names[ava_key].name + " -> ");
                else Output.Write(ava_key.ToString());
                Output.WriteLine("\n  " + com_data.line);
            }

            DoggyMCP.SL_Client_Sub.SL_Message message = MCP_Object_Generate(ava_key, session_id, line);
            message.message_art = SL_Client_Sub.SL_Message.Message_art.im;
            Net_Sub.Message_Fire.to_all(message);

        }

        private static string last_g_im = "";
        public static void g_im(UUID ava_key, UUID session_id, string line)
        {
            if (!Data.G_IMs.ContainsKey(ava_key)) Data.G_IMs.Add(ava_key, new List<Com_data>());

            Com_data com_data = new Com_data();
            com_data.session_id = session_id;
            com_data.line = line;

            Data.G_IMs[ava_key].Add(com_data);

            if ((DoggyMCP_main.verbose) && (last_g_im!= com_data.line))
            {
                last_g_im = com_data.line;
                Output.Write("Group-IM for ");
                if (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key)) Output.Write(DoggyMCP.SL_Client_Sub.Data.names[ava_key].name + " -> ");
                else Output.Write(ava_key.ToString() + " -> ");
                if (Data.groups.ContainsKey(session_id)) Output.Write(Data.groups[session_id].group_name);
                else Output.Write(session_id.ToString());
                Output.WriteLine("\n  " + com_data.line);
            }

            DoggyMCP.SL_Client_Sub.SL_Message message = MCP_Object_Generate(ava_key, session_id, line);
            message.message_art = SL_Client_Sub.SL_Message.Message_art.g_im;
            Net_Sub.Message_Fire.to_all(message);
        }

        private static DoggyMCP.SL_Client_Sub.SL_Message MCP_Object_Generate (UUID ava_key, UUID session_id, string line)
        {
            DoggyMCP.SL_Client_Sub.SL_Message message = new SL_Client_Sub.SL_Message();
            message.agent_id = ava_key;
            message.session_id = session_id;
            message.message = line;
            return(message);
        }

    }
}
