using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyMCP.Logs_sub
{
    class Logs
    {
        public static Dictionary<UUID, List<string>> logs_ava = new Dictionary<UUID, List<string>>();

        public static void init(UUID key)
        {
            if (logs_ava.Keys.Contains(key)) logs_ava.Remove(key);

            logs_ava.Add(key, new List<string>());

            Output.WriteLine(key.ToString() + " -> NEW AVA");
        }

        public static void add(UUID key, string line)
        {
            if (!logs_ava.Keys.Contains(key)) init(key);

            logs_ava[key].Add(line);

            Output.WriteLine(key.ToString() + " -> " + line);

        }
    }
}
