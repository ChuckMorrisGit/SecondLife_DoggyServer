using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Messages.Linden;
using OpenMetaverse.Assets;
using FreeImageAPI;
using OpenMetaverse.Imaging;
//using convert2jpg;


namespace DoggyServer.Inventory_Sub.Rake_Sub
{
    class Rake
    {
        private static GridClient Client = new GridClient();
        private static string _firstName = string.Empty;
        private static string _lastName = string.Empty;

        private static readonly Thread RakeThread = new Thread(ProcessInventory);
        private static readonly List<bool> StabilizeChecks = new List<bool>();
        private static AutoResetEvent _folders = new AutoResetEvent(false);

        private static bool _overwriteExisting = false;
        public static bool rake_objects = false;
        private static Dictionary<InventoryItem, RakeObject> rez_objects = new Dictionary<InventoryItem, RakeObject>();

        private static Dictionary<UUID, InventoryBase> folders = new Dictionary<UUID, InventoryBase>();

        public static void run(GridClient client)
        {
            Client = client;

            _firstName = Client.Self.FirstName;
            _lastName = Client.Self.LastName;



            RakeThread.Start();
        }

        private static void HandleEventQueueRunning(object sender, EventQueueRunningEventArgs e)
        {
            Client.Network.EventQueueRunning -= HandleEventQueueRunning;
            Output_sub.Logs.add("\n[Rake] : Event queue started...", false);
            StabilizeChecks.Add(true);
        }

        private static void HandleSimulatorConnected(object sender, SimConnectedEventArgs e)
        {
            Client.Network.SimConnected -= HandleSimulatorConnected;
            Output_sub.Logs.add("\n[Rake] : Simulator connected...", false);
            StabilizeChecks.Add(true);
        }

        private static void HandleAppearanceSet(object sender, AppearanceSetEventArgs e)
        {
            if (!e.Success) return;
            Client.Appearance.AppearanceSet -= HandleAppearanceSet;
            Output_sub.Logs.add("\n[Rake] : Appearance set...", false);
            StabilizeChecks.Add(true);
        }

        private static void HandleLoginProgress(object sender, LoginProgressEventArgs e)
        {
            if (e.Status == LoginStatus.Success)
            {
                Output_sub.Logs.add("\n[Rake] : Login ok...", false);
                StabilizeChecks.Add(true);
            }
            switch (e.Status)
            {
                case LoginStatus.Failed:
                    Output_sub.Logs.add("\n[Rake] : Failed Login...\n", false);
                    break;
            }
        }

        private static void ProcessInventory()
        {
            Output_sub.Logs.add("\n[Rake] : Stabilizing, please wait...", false);

            /*
            while (StabilizeChecks.Count < 4)
            {
                switch (Client.Network.LoginStatusCode)
                {
                    case LoginStatus.Failed:
                        return;
                }
                Thread.Sleep(1000);
                Output_sub.Logs.add("Stabilizing, please wait...", false);
            }
            */

            Output_sub.Logs.add("\n[Rake] : Raking: ", false);
            RakeInventory(Client.Inventory.Store.RootFolder);
            new List<int>(new int[Console.WindowWidth]).ForEach(i => Output_sub.Logs.add("-", false));

            //Wait for all Objects
            System.Threading.Thread.Sleep(10000);
            while (rez_objects.Count > 0)
            {
                System.Threading.Thread.Sleep(10000);
            }

            Output_sub.Logs.add("[Rake] : All operations completed.", false);
            Client.Network.Logout();
        }

        private static System.Threading.Thread rez_thread = null;

        private static void RakeInventory(InventoryBase root)
        {
            Output_sub.Logs.add("RAKE Folder: " + root.Name, false);


            if (!folders.ContainsKey(root.UUID)) folders.Add(root.UUID, root);

            _folders = new AutoResetEvent(false);
            Client.Inventory.FolderUpdated +=
                delegate(object sender, FolderUpdatedEventArgs e) { if (e.Success) _folders.Set(); };
            _folders.Reset();
            Client.Inventory.RequestFolderContents(root.UUID, Client.Self.AgentID, true, true, InventorySortOrder.ByName);
            _folders.WaitOne(60000, false);

            var inventoryBases = Client.Inventory.FolderContents(root.UUID, Client.Self.AgentID,
                                                                 true, true, InventorySortOrder.ByName, 60000);
            if (inventoryBases == null) return;

            foreach (var ib in inventoryBases)
            {
                Thread.Sleep(Client.Network.CurrentSim.Stats.LastLag + (int)Math.Round(new ViewerStatsMessage().AgentPing));
                var done = new AutoResetEvent(false);

                if (ib is InventoryFolder)
                {
                    var folder = ib as InventoryFolder;

                    if ((folder.PreferredType != AssetType.TrashFolder) && (folder.PreferredType != AssetType.LostAndFoundFolder))
                    {
                        RakeInventory(folder);
                    }
                    continue;
                }

                if (!(ib is InventoryItem)) continue;
                var item = ib as InventoryItem;

                List<string> pathParts_list = new List<string>();

                UUID current_UUID = root.UUID;

                while (folders[current_UUID].ParentUUID != UUID.Zero)
                {
                    pathParts_list.Add(Path.GetInvalidFileNameChars().Aggregate(folders[current_UUID].Name, (current, c) => current.Replace(c.ToString(), string.Empty)));
                    current_UUID = folders[current_UUID].ParentUUID;
                }

                pathParts_list.Add(_firstName + " " + _lastName);

                pathParts_list.Reverse();
                string[] pathParts = pathParts_list.ToArray();

                string path_save = Path.Combine(pathParts);

                if (!Directory.Exists(path_save))
                    Directory.CreateDirectory(path_save);


                switch (item.AssetType)
                {
                    case AssetType.Bodypart:
                        break;

                    case AssetType.Clothing:

                        #region Clothing Rake

                        var clothingNormal = Path.GetInvalidFileNameChars().Aggregate(item.Name, (current, c) => current.Replace(c.ToString(), string.Empty));
                        int counter = 0;

                        if (!_overwriteExisting && File.Exists(Path.Combine(path_save, clothingNormal + "-" + counter.ToString() + ".tga")))
                        {
                            break;
                        }

                        Client.Assets.RequestAsset(item.AssetUUID, AssetType.Clothing, false,
                            (transfer, asset) =>
                            {
                                if (!transfer.Success)
                                {
                                    Output_sub.Logs.add("ERROR RAKE CLOTHING DOWNLOADING", false);
                                    done.Set();
                                    return;
                                }

                                AssetClothing ac = asset as AssetClothing;

                                ac.Decode();

                                foreach (UUID tex in ac.Textures.Values)
                                {

                                    //File.WriteAllBytes(Path.Combine(path_save, clothingNormal + "-" + counter.ToString() + ".jp2-data"), asset.AssetData);

                                    //Asset_Sub.Image.get(Client, path_save, tex);
                                    try
                                    {
                                        Output_sub.Logs.add("RAKE CLOTHING REQUEST TEXTURE: " + tex.ToString(), false);
                                        Client.Assets.RequestImage(tex, (clothingState,  clothingTextureAsset) =>
                                        {
                                            if (clothingState == TextureRequestState.Finished)
                                            {
                                                ManagedImage imgData;
                                                OpenJPEG.DecodeToImage(clothingTextureAsset.AssetData, out imgData);

                                                byte[] tgaFile = imgData.ExportTGA();

                                                File.WriteAllBytes(Path.Combine(path_save, clothingNormal + "-" + counter.ToString() + ".tga"), tgaFile);
                                            }
                                        });
                                    }
                                    catch (Exception ex) { Output_sub.Logs.add("ERROR RAKE CLOTHING TEXTURE: " + ex.Message, false); }

                                    counter++;

                                    Output_sub.Logs.add("RAKE CLOTHING DOWNLOADED: " + clothingNormal, false);
                                }

                                done.Set();
                            });
                        done.WaitOne(60000, false);

                        #endregion Clothing Rake

                        break;

                    case AssetType.Texture:

                        #region Texture Rake

                        var imageTexture = ib as InventoryTexture;
                        var imageSnapshot = ib as InventorySnapshot;

                        string textureFileName;
                        UUID textureAssetUUID;
                        if (imageTexture != null)
                        {
                            textureFileName = Path.GetInvalidFileNameChars().Aggregate(imageTexture.Name + ".tga", (current, c) => current.Replace(c.ToString(), string.Empty));
                            textureAssetUUID = imageTexture.AssetUUID;
                            goto download_texture;
                        }

                        if (imageSnapshot != null)
                        {
                            textureFileName = Path.GetInvalidFileNameChars().Aggregate(imageSnapshot.Name + ".tga", (current, c) => current.Replace(c.ToString(), string.Empty));
                            textureAssetUUID = imageSnapshot.AssetUUID;
                            goto download_texture;
                        }
                        break;

                    download_texture:
                        textureFileName = Path.Combine(path_save, textureFileName);

                        if (!_overwriteExisting && File.Exists(textureFileName))
                        {
                            break;
                        }

                        Client.Assets.RequestImage(textureAssetUUID, ImageType.Normal, (state, asset) =>
                        {
                            if (state != TextureRequestState.Finished)
                            {
                                Output_sub.Logs.add("ERROR RAKE TEXTURE DOWNLOADING", false); 
                                return;
                            }

                            //File.WriteAllBytes(textureFileName, asset.AssetData);

                            ManagedImage imgData;
                            OpenJPEG.DecodeToImage(asset.AssetData, out imgData);

                            byte[] tgaFile = imgData.ExportTGA();

                            File.WriteAllBytes(textureFileName, tgaFile);

                            Output_sub.Logs.add("RAKE TEXTURE DOWNLOADED: " + textureFileName, false);
                            done.Set();
                        }, false);

                        done.WaitOne(60000, false);

                        #endregion Texture Rake

                        break;
                    case AssetType.Notecard:

                        #region Notecard Rake

                        var notecard = (InventoryNotecard)ib;

                        var notecardNormal = Path.GetInvalidFileNameChars().Aggregate(notecard.Name + ".rtf", (current, c) => current.Replace(c.ToString(), string.Empty));

                        notecardNormal = Path.Combine(path_save, notecardNormal);

                        if (!_overwriteExisting && File.Exists(notecardNormal))
                        {
                            break;
                        }
                        Client.Assets.RequestInventoryAsset(notecard.AssetUUID, notecard.UUID, UUID.Zero, Client.Self.AgentID, AssetType.Notecard, true,
                            (transfer, asset) =>
                            {
                                if (!transfer.Success)
                                {
                                    Output_sub.Logs.add("ERROR RAKE NOTECARD DOWNLOADING", false);
                                    done.Set();
                                    return;
                                }
                                File.WriteAllBytes(notecardNormal, asset.AssetData);
                                Output_sub.Logs.add("RAKE NOTECARD DOWNLOADED: " + notecardNormal, false);
                                done.Set();
                            });
                        done.WaitOne(60000, false);

                        #endregion Notecard Rake

                        break;
                    case AssetType.Animation:

                        #region Animation Rake

                        var animation = (InventoryAnimation)ib;

                        var animationNormal = Path.GetInvalidFileNameChars().Aggregate(animation.Name + ".animatn", (current, c) => current.Replace(c.ToString(), string.Empty));

                        animationNormal = Path.Combine(path_save, animationNormal);
                        if (!_overwriteExisting && File.Exists(animationNormal))
                        {
                            break;
                        }
                        Client.Assets.RequestInventoryAsset(animation.AssetUUID, animation.UUID, UUID.Zero, Client.Self.AgentID, AssetType.Animation, true,
                            (transfer, asset) =>
                            {
                                if (!transfer.Success)
                                {
                                    Output_sub.Logs.add("ERROR RAKE ANIMATION DOWNLOADING", false);
                                    done.Set();
                                    return;
                                }
                                File.WriteAllBytes(animationNormal, asset.AssetData);
                                Output_sub.Logs.add("RAKE ANIMATION DOWNLOADED: " + animationNormal, false);
                                done.Set();
                            });
                        done.WaitOne(60000, false);

                        #endregion Animation Rake

                        break;
                    case AssetType.LSLText:

                        #region Script Rake

                        var script = (InventoryLSL)ib;
                        if (script.Permissions.OwnerMask != PermissionMask.All) break;

                        var scriptNormal = Path.GetInvalidFileNameChars().Aggregate(script.Name + ".lsl", (current, c) => current.Replace(c.ToString(), string.Empty));
                        scriptNormal = Path.Combine(path_save, scriptNormal);

                        if (!_overwriteExisting && File.Exists(scriptNormal))
                        {
                            break;
                        }

                        Client.Assets.RequestInventoryAsset(script.AssetUUID, script.UUID, UUID.Zero, Client.Self.AgentID, AssetType.LSLText, true, (transfer, asset) =>
                        {
                            if (!transfer.Success)
                            {
                                Output_sub.Logs.add("ERROR RAKE SCRIPT DOWNLOADING", false);
                                done.Set();
                                return;
                            }

                            File.WriteAllBytes(scriptNormal, asset.AssetData);
                            Output_sub.Logs.add("RAKE LSL DOWNLOADED: " + scriptNormal, false);
                            done.Set();
                        });

                        done.WaitOne(60000, false);

                        #endregion Script Rake

                        break;

                    case AssetType.Object:

                        #region Object Rake
                        if (rake_objects)
                        {

                            var objectNormal = Path.GetInvalidFileNameChars().Aggregate(item.Name + ".xml", (current, c) => current.Replace(c.ToString(), string.Empty));
                            objectNormal = Path.Combine(path_save, objectNormal);

                            if (!_overwriteExisting && File.Exists(objectNormal))
                            {
                                break;
                            }

                            RakeObject rakeObject = new RakeObject();
                            rakeObject.filename = objectNormal;
                            rez_objects.Add(item, rakeObject);
                            Output_sub.Logs.add("ADD OBJECT TO REZ: " + item.Name, false);

                            try
                            {
                                if ((rez_thread == null) || (!rez_thread.IsAlive))
                                {
                                    rez_thread = new System.Threading.Thread(new System.Threading.ThreadStart(rez_thread_Run));
                                    rez_thread.IsBackground = true;
                                    rez_thread.Start();
                                }

                            }
                            catch (Exception ex) { Output_sub.Logs.add("THREAD ERROR DEREZ: " + ex.Message, false); }


                        }
                        #endregion

                        break;
                }
            }
        }

        private static void rez_thread_Run()
        {
            while (rez_objects.Count > 0)
            {
                try
                {
                    InventoryItem item = rez_objects.Keys.First();

                    //UUID primID = Client.Inventory.RequestRezFromInventory(Client.Network.CurrentSim, Client.Self.SimRotation, Client.Self.SimPosition, item);

                    Client.Appearance.Attach(item, AttachmentPoint.Root,true);
                    Output_sub.Logs.add("Attach: " + item.Name,false);

                    rez_objects.Remove(item);


                    // Raken von dem Attachted Object



                    System.Threading.Thread.Sleep(2000);
                    Output_sub.Logs.add("Detach: " + item.Name, false);

                    Client.Appearance.Detach(item);
                    System.Threading.Thread.Sleep(2000);


                }
                catch (Exception ex) { Output_sub.Logs.add("ERROR DEREZ: " + ex.Message, false); }

            }
        }

    }
}