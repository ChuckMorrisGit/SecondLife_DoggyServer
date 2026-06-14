using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class Dump
    {
        private static GridClient client;
        public static void all(GridClient client_new)
        {
            client = client_new;

            subSir(client.Inventory.Store.RootFolder.UUID, new List<string>());
        }

        public static void subSir(UUID folder, List<string> folders)
        {
            try
            {
                List<InventoryBase> contents = client.Inventory.FolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByDate, 3000);

                foreach (InventoryBase item in contents)
                {
                    if (item is InventoryFolder)
                    {
                        folders.Add(item.Name);
                        subSir(item.UUID, folders);
                    }
                    else
                    {
                        Output_sub.Logs.add(item.Name, false);
                    }
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("INVENTORY: " + ex.Message, false); }
        }
    }
}
