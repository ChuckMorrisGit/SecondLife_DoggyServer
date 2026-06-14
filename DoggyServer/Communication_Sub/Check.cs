using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Check
    {
        public static void what(GridClient client, string[] commandArray, UUID fromAV, string fromName)
        {
            try
            {
                switch (commandArray[1])
                {
                    case "outfit":
                        Inventory_Sub.Check.avaOutfit(client);
                        break;

                }
            }
            catch (Exception ex) { client.Self.InstantMessage(fromAV, "More params? -> " + ex.Message); }
        }

        public static int ifChannel(string text)
        {
            int channel = 0;
            
            string firstChar = text.Substring(0, 1);
            if (firstChar == "/")
            {
                string channelText = text.Substring(1, text.Length -1);
                int.TryParse(channelText, out channel);
            }

            return (channel);
        }
    }
}
