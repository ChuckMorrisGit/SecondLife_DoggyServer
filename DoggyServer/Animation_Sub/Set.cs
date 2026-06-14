using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Animation_Sub
{
    class Set
    {
        public static void on(GridClient client)
        {
            client.Self.Chat("AOON", 789, ChatType.Normal);
        }
        public static void off(GridClient client)
        {
            client.Self.Chat("AOOFF", 789, ChatType.Normal);
        }
        public static void reset(GridClient client)
        {
            client.Self.Chat("AOreset", 789, ChatType.Normal);
        }
    }
}
