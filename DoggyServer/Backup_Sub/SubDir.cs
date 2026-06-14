using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Backup_Sub
{
    class SubDir
    {
        public static void subDir(GridClient client, UUID folder)
        {
            client.Inventory.RequestFolderContents(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate);
            List<InventoryBase> contents = client.Inventory.FolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByDate, 3000);

            if (contents != null)
            {
                foreach (InventoryBase itemBase in contents)
                {
                    if (itemBase is InventoryItem)
                    {
                        InventoryItem item = (InventoryItem)itemBase;

                        switch (item.AssetType)
                        {
                            case AssetType.Animation:
                                Asset_Sub.Ani.download(client, item.AssetUUID, item.Name);
                                break;
                        }
                    }
                }
            }
        }
    }
}
