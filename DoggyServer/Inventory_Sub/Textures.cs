using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class Textures
    {
        private static Dictionary<UUID, string> texItems = new Dictionary<UUID, string>();
        public static void toTXT(GridClient client)
        {

            subDir(client, client.Inventory.Store.RootFolder.UUID);
            subDir(client, client.Inventory.Store.RootFolder.UUID);
            subDir(client, client.Inventory.Store.RootFolder.UUID);

            StreamWriter writer = new StreamWriter("InventoryTextures - " + client.Self.Name + ".txt");

            foreach (UUID key in texItems.Keys)
            {
                writer.WriteLine(key.ToString() + "," + texItems[key]);
            }

            writer.Close();
        }

        private static string currentFolter = string.Empty;
        private static void subDir(GridClient client, UUID folder)
        {
            client.Inventory.RequestFolderContents(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate);
            List<InventoryBase> contents = client.Inventory.FolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByDate, 3000);

            if (contents != null)
            {

                foreach (InventoryBase item in contents)
                {
                    if (item is InventoryFolder)
                    {
                        subDir(client, item.UUID);
                    }

                    if (item is InventoryItem)
                    {
                        InventoryItem itemAsset = item as InventoryItem;
                        switch (itemAsset.AssetType)
                        {
                            case AssetType.Texture:
                            case AssetType.TextureTGA:
                                if (!texItems.ContainsKey(itemAsset.AssetUUID))
                                {
                                    texItems.Add(itemAsset.AssetUUID, itemAsset.Name);
                                    Output_sub.Logs.add("FOUND TEXTURE: " + itemAsset.Name, false);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
