using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Animation_Sub
{
    class Data
    {
        public static List<UUID> stands = new List<UUID>();
        public static List<UUID> walks = new List<UUID>();

        public static void init (GridClient client)
        {
            stands.Add(new UUID("9edf6668-fa3c-fc9a-88d5-3cf58ac2ff12"));
            stands.Add(new UUID("027e5861-1f69-60bb-6f4c-5d5ee30bd7d9"));
            stands.Add(new UUID("5394c151-3dca-670c-1857-75725726e36c"));
            stands.Add(new UUID("5394c151-3dca-670c-1857-75725726e36c"));
            stands.Add(new UUID("b7bf4023-8ee3-43a4-c9b4-cf2979bd9631"));

            walks.Add(new UUID("38905fd8-a677-41b0-7ead-13549cd7703b"));
            walks.Add(new UUID("7e81953d-4338-5f93-8d04-0b5d83ac746c"));

        }
    }
}
