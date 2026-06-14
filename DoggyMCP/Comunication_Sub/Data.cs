using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyMCP.Comunication_Sub
{
    [Serializable()]
    class Data
    {
        public static Dictionary<UUID, List<Com_data>> chats = new Dictionary<UUID, List<Com_data>>();
        public static Dictionary<UUID, List<Com_data>> IMs = new Dictionary<UUID, List<Com_data>>();
        public static Dictionary<UUID, List<Com_data>> G_IMs = new Dictionary<UUID, List<Com_data>>();

        public static Dictionary<UUID, Group_data> groups = new Dictionary<UUID, Group_data>();

        public static Dictionary<UUID, List<string>> say_chat = new Dictionary<UUID, List<string>>();
        public static Dictionary<UUID, List<Com_data>> say_im = new Dictionary<UUID, List<Com_data>>();
    }

    [Serializable()]
    public class Com_data
    {
        public UUID session_id = UUID.Zero;
        public string line = string.Empty;
        public DateTime timestamp = DateTime.Now;
    }

    [Serializable()]
    public class Group_data
    {
        public UUID group_id = UUID.Zero;
        public string group_name = string.Empty;
    }

}
