using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class Offered
    {
        private static GridClient client;
        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Inventory.InventoryObjectOffered += new EventHandler<InventoryObjectOfferedEventArgs>(Inventory_InventoryObjectOffered);
            client.Inventory.ItemReceived += new EventHandler<ItemReceivedEventArgs>(Inventory_ItemReceived);

        }

        static void Inventory_ItemReceived(object sender, ItemReceivedEventArgs e)
        {
            switch (e.Item.AssetType)
            {
            }
        }

        static void Inventory_InventoryObjectOffered(object sender, InventoryObjectOfferedEventArgs e)
        {
            e.Accept = true;


            Communication_Sub.MasterChat.reply(client, "Get Object: " + e.AssetType.ToString());

            try
            {
                switch (e.AssetType)
                {
                    case AssetType.Texture:
                    case AssetType.TextureTGA:
                    case AssetType.ImageJPEG:
                    case AssetType.ImageTGA:

                        InventoryItem item = client.Inventory.FetchItem(e.ObjectID, client.Self.AgentID, 3000);
                        if (item.Name.ToLower().Contains("caprica")) Database_Sub.Insert2Marketplace.Insert2Item(item);
                        Asset_Sub.Image.get(client, item.Name + " Inventory", item.AssetUUID);
                        Communication_Sub.MasterChat.reply(client, "Caprica Shop Texture added 2 Database: " + item.Name);
                        //client.Self.InstantMessage(new UUID("42e273af-0cc3-40b2-91cd-45e62b95ef4b"), "Download Image: " + item.AssetUUID.ToString());
                        break;

                    case AssetType.Folder:
                        List<InventoryBase> contents = client.Inventory.FolderContents(e.ObjectID, client.Self.AgentID, true, true, InventorySortOrder.ByDate, 3000);
                        foreach (InventoryBase itemBase in contents)
                        {

                            if (itemBase is InventoryItem)
                            {
                                InventoryItem itemItem = (InventoryItem)itemBase;
                                Asset_Sub.Image.get(client, "Inventory", itemItem.AssetUUID);
                                Communication_Sub.MasterChat.reply(client, "Download Image: " + itemItem.AssetUUID.ToString());
                            }
                        }
                        break;

                    case AssetType.Clothing:
                        InventoryItem item_cloth = client.Inventory.FetchItem(e.ObjectID, client.Self.AgentID, 3000);
                        Asset_Sub.Item.download(client, AssetType.Clothing, item_cloth.AssetUUID);
                        client.Self.InstantMessage(new UUID("11111111-2222-3333-4444-555555555555"), "getto: " + item_cloth.AssetUUID.ToString());
                        break;

                    case AssetType.LSLText:
                        InventoryItem item_script = client.Inventory.FetchItem(e.ObjectID, client.Self.AgentID, 3000);
                        Asset_Sub.Item.download(client, e.AssetType, item_script.AssetUUID);

                        break;
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("GET ITEM ERROR: " + ex.Message, false); }

        }
    }
}
