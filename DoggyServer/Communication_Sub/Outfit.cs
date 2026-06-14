using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Outfit
    {
        public static void set(GridClient client, string[] args, UUID fromAva, string fromName)
        {
            try
            {
                switch (args[1].ToLower())
                {
                    case "ao":
                        client.Appearance.AddToOutfit(Inventory_Sub.Data.AO.First().Value);
                        break;

                }
            }
            catch (Exception ex) { Output_sub.Logs.add("Outfit Set ERROR: " + ex.Message, false); }
        }
    }
}
