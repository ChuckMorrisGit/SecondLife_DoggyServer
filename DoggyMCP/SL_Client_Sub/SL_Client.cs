using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyMCP.SL_Client_Sub
{
    class Data
    {
        public static Dictionary<UUID, SL_Client> names = new Dictionary<UUID, SL_Client>();
    }

    [Serializable()]
    public class SL_Client
    {
        public UUID id = UUID.Zero;
        public string name = string.Empty;
        public Boolean online = false;
        public Boolean follow = false;
        public string current_master = string.Empty;
        public Boolean alice_chat = false;
        public Boolean alice_im = false;
        public Boolean alice_g_im = false;
        public Boolean chat_follow = false;
    }

    [Serializable()]
    public class SL_Message
    {
        public enum Message_art
        {
            none = 0,
            chat = 1,
            im = 2,
            g_im = 3,
        }

        public UUID agent_id = UUID.Zero;
        public UUID session_id = UUID.Zero;
        public string message = string.Empty;
        public Message_art message_art = Message_art.none;
    }

    [Serializable()]
    public class SL_Notify
    {
        public enum Notify_art
        {
            none = 0,
            LOGIN = 1,
            LOGOUT = 2,
            CONTROL = 3,
        }

        public UUID agent_id = UUID.Zero;
        public Notify_art notify_art = Notify_art.none;
    }
}
