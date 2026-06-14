using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
namespace DoggyServer.Inventory_Sub
{
    class Delete
    {
        public static void norm(GridClient client, UUID fromAV, string fromName)
        {
            client.Inventory.RequestFolderContents(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate);

            List<InventoryBase> contents = client.Inventory.FolderContents(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate, 3000);

            client.Self.InstantMessage(fromAV, "deleting #norm* Folder");
            if (contents != null)
            {

                foreach (InventoryBase item in contents)
                {
                    if (item is InventoryFolder)
                    {
                        if (item.Name.Contains("#norm"))
                        {
                            client.Inventory.MoveFolder(item.UUID, client.Inventory.FindFolderForType(AssetType.TrashFolder));
                            client.Self.InstantMessage(fromAV, "deleted: " + item.Name);
                        }
                    }
                }
            }

            client.Self.InstantMessage(fromAV, "deleting finished. Reload Inventory");
            Fetch.all(client, fromAV, false);
            client.Self.InstantMessage(fromAV, "done");
        }
    }
}
