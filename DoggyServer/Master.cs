using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer
{
    class Master
    {
        public static List<UUID> firstLevelMaster = new List<UUID>();
        public static List<UUID> secondLevelMaster = new List<UUID>();

        public static Dictionary<string, AvaData> masters = new Dictionary<string, AvaData>();

        public static string currentMaster = string.Empty;

        public static void init()
        {

        }
    }
}
