using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class Check
    {
        private static GridClient client;
        public static void avaOutfit(GridClient client_new)
        {
            client = client_new;

            subSir(client.Inventory.Store.RootFolder.UUID, "root");
        }

        public static void subSir(UUID folder, string folderName)
        {
            try
            {
                List<InventoryBase> contents = client.Inventory.FolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByDate, 3000);

                foreach (InventoryBase item in contents)
                {
                    if (folderName == "Doggy")
                    {
                        bool worn = client.Appearance.IsItemWorn((InventoryItem) item) != WearableType.Invalid;

                        Output_sub.Logs.add(item.Name + ": " +worn.ToString(), false);
                    }

                    if (item is InventoryFolder)
                    {
                        subSir(item.UUID, item.Name);
                    }
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("INVENTORY: " + ex.Message, false); }
        }


    }
}
