using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class NormAppearance
    {
        public static List<InventoryBase> normOutfit = new List<InventoryBase>();
        private static string standartordner = "#norm0";
        private static string aoordner = "#ao";
        public static void init(GridClient client)
        {
        }

        public static void set(GridClient client, UUID fromAva, string targetOutfit)
        {
            List<string> norms = new List<string>();
            norms.Add(targetOutfit);
            norms.Add(standartordner);
            norms.Add(aoordner);
            List<InventoryItem> items = new List<InventoryItem>();

            foreach (string target in norms)
            {
                //string target = "#norm";
                client.Inventory.RequestFolderContents(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate);

                UUID folder = client.Inventory.FindObjectByPath(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, target, 20 * 1000);

                if (folder != UUID.Zero)
                {
                    client.Inventory.RequestFolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByDate);

                    List<InventoryBase> contents = client.Inventory.FolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByName, 20 * 1000);

                    if (contents != null)
                    {
                        string attachedItems = "";
                        foreach (InventoryBase item in contents)
                        {
                            if (item is InventoryItem)
                            {
                                items.Add((InventoryItem)item);
                                attachedItems += item.Name + ", ";
                            }
                        }
                        if (fromAva != UUID.Zero) client.Self.InstantMessage(fromAva, attachedItems + "attached");
                    }
                    else { Output_sub.Logs.add("#norm Folder not readable", false); }
                }
                else { Output_sub.Logs.add("ROOT Folder not readable", false); }
            }

            if (items.Count != 0) client.Appearance.ReplaceOutfit(items);
        }
    }
}
