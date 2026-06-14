using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class Data
    {
        public static Dictionary<UUID, InventoryItem> items = new Dictionary<UUID, InventoryItem>();

        public static Dictionary<string, List<InventoryItem>> outfits = new Dictionary<string, List<InventoryItem>>();
        public static Dictionary<string, List<InventoryItem>> AO = new Dictionary<string, List<InventoryItem>>();
    }
}
