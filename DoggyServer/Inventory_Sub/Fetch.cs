using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Inventory_Sub
{
    class Fetch
    {
        private static Boolean output = false;
        private static Thread fetch_inventory;
        public static Boolean fetching = false;

        public static void all(GridClient client, UUID imTo, Boolean output_console)
        {
            output = output_console;

            if (!fetching)
            {
                fetch_inventory = new Thread(delegate() { all_run(client, imTo, output_console); });
                fetch_inventory.IsBackground = true;
                fetch_inventory.Priority = ThreadPriority.Normal;
                fetch_inventory.Start();
            }
        }

        private static void all_run(GridClient client, UUID imTo, Boolean output_console)
        {
            fetching = true;
            
            subDir(client, client.Inventory.Store.RootFolder.UUID, imTo);
            client.Self.InstantMessage(imTo, "Inventory fetched: " + Inventory_Sub.Data.items.Count().ToString() + " items");
            Output_sub.Logs.add ("Inventory fetched: " + Inventory_Sub.Data.items.Count().ToString() + " items", false);
            fetching = false;

        }

        private static string currentFolter = string.Empty;
        private static void subDir(GridClient client, UUID folder, UUID imTo)
        {
            //client.Inventory.RequestFolderContents(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate);
            List<InventoryBase> contents = client.Inventory.FolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByDate, 3000);

            //if (output) Output_sub.Logs.add("AVA ROOT DIR: " + client.Inventory.Store.RootFolder.UUID.ToString(), false);

            if (contents != null)
            {

                foreach (InventoryBase item in contents)
                {
                    try
                    {

#region Read Items
                        if (item is InventoryItem)
                        {
                            try
                            {
                                //if (output) Output_sub.Logs.add("Fetch ITEM: " + item.Name, false);
                                //if (output) Output_sub.Logs.add("Fetch ITEM: " + item.UUID.ToString(), false);

                                if (!Data.items.ContainsKey(item.UUID))
                                    Data.items.Add(item.UUID, (InventoryItem)item);

                                if (currentFolter.ToLower().Contains("#norm"))
                                {
                                    if (!Data.outfits.ContainsKey(currentFolter))
                                    {
                                        if (imTo != UUID.Zero) client.Self.InstantMessage(imTo, "Found: " + currentFolter);
                                        List<InventoryItem> items = new List<InventoryItem>();
                                        Data.outfits.Add(currentFolter, items);
                                        //if (output) Output_sub.Logs.add("Fetch ITEM: " + item.Name, false);
                                    }
                                    Data.outfits[currentFolter].Add((InventoryItem)item);
                                }

                                if (currentFolter.ToLower().Contains("#ao"))
                                {
                                    if (!Data.AO.ContainsKey(currentFolter))
                                    {
                                        List<InventoryItem> items = new List<InventoryItem>();
                                        Data.AO.Add(currentFolter, items);
                                    }
                                    Data.AO[currentFolter].Add((InventoryItem)item);
                                }

                                if (currentFolter.ToLower().Contains("#get"))
                                {


                                }

                                
                            }
                            catch (Exception ex) { Output_sub.Logs.add("FETCH ITEM ERROR: " + ex.Message, false); }
                        }
                        #endregion

#region Read Folders    
                        if (item is InventoryFolder)
                        {
                            try
                            {
                                currentFolter = item.Name;
                                //if (output) Output_sub.Logs.add("Fetch FOLDER: " + item.Name, false);
                                //if (output) Output_sub.Logs.add("Fetch FOLDER: " + item.UUID.ToString(), false);

                                switch (currentFolter)
                                {
                                    case "#Phoenix":
                                        Backup_Sub.SubDir.subDir(client, item.UUID);
                                        Output_sub.Logs.add("FOUND SUBDIR: " + currentFolter, false);
                                        client.Inventory.RemoveFolder(folder);
                                        break;

                                    case "#Firestorm":
                                    case "#Emerald":
                                        Output_sub.Logs.add("FOUND SUBDIR: " + currentFolter, false);
                                        client.Inventory.RemoveFolder(folder);
                                        break;

                                }

                                subDir(client, item.UUID, imTo);
                            }
                            catch (Exception ex) { Output_sub.Logs.add("FETCH FOLDER ERROR: " + ex.Message, false); }
                        }
#endregion



                    }
                    catch (Exception ex) { Output_sub.Logs.add("FETCH ERROR: " + ex.Message, false); }
                }
            }
        }
    }
}
