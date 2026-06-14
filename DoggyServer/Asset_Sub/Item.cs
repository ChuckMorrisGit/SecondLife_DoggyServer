using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Assets;
using System.IO;
using System.Threading;

namespace DoggyServer.Asset_Sub
{
    class Item
    {
        public static string subDir = "./rip/";
        private static GridClient client;
        private static AutoResetEvent DownloadHandle = new AutoResetEvent(false);

        public static void init(GridClient client_new)
        {
            client = client_new;
        }

        public static void close(GridClient client_new)
        {
        }

        public static void download(GridClient client, AssetType assetType, UUID assetUUID)
        {
            try
            {
                DownloadHandle.Reset();

                Output_sub.Logs.add("Try download: " + assetType.ToString(), false);

                switch (assetType)
                {
                    case AssetType.Clothing:
                        client.Assets.RequestAsset(assetUUID, AssetType.Clothing, false, new AssetManager.AssetReceivedCallback(Assets_OnClothesReceived));
                        break;

                    default:
                        client.Assets.RequestAsset(assetUUID, assetType, true, Assets_OnAssetReceived);
                        //client.Assets.RequestAsset(assetUUID, assetType, false, new AssetManager.AssetReceivedCallback(Assets_OnScriptReceived));
                        break;
                }

                if (DownloadHandle.WaitOne(120 * 1000, false))
                {
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("ASSET DOWNLOAD ERROR: " + ex.Message, false); }
        }

        private static void Assets_OnClothesReceived(AssetDownload transfer, OpenMetaverse.Assets.Asset asset)
        {
            AssetClothing c = asset as AssetClothing;

            c.Decode();

            //Communication_Sub.MasterChat.reply(client, "TEXT: " + c.Textures.ToString());
            //Communication_Sub.MasterChat.reply(client, "NAME: " + c.Name);

            foreach (UUID tex in c.Textures.Values)
            {
                Image.get(client, "Inventory-Clothes ", tex);
                Communication_Sub.MasterChat.reply(client, "Download Image: " + tex.ToString());
                client.Self.Chat(tex.ToString(), 666, ChatType.Shout);
                //client.Self.Chat(tex.ToString(), 0, ChatType.Shout);
            }

            DownloadHandle.Set();
            Output_sub.Logs.add("Clothes downloaded: " + asset.AssetID.ToString(), false);
        }

        private static void Assets_OnScriptReceived(AssetDownload transfer, OpenMetaverse.Assets.Asset asset)
        {
            AssetScriptText c = asset as AssetScriptText;

            c.Decode();

            //Communication_Sub.MasterChat.reply(client, "TEXT: " + c.Textures.ToString());
            //Communication_Sub.MasterChat.reply(client, "NAME: " + c.Name);

            DownloadHandle.Set();

            Output_sub.Logs.add("Script downloaded: " + c.Source, false);
        }

        private static void Assets_OnAssetReceived(AssetDownload transfer, Asset asset)
        {
            if (transfer.Success)
            {
                try
                {
                    if (!Directory.Exists(subDir)) Directory.CreateDirectory(subDir);

                    File.WriteAllBytes(subDir + String.Format("{0}.{1}", transfer.AssetID,
                        transfer.AssetType.ToString().ToLower()), transfer.AssetData);


                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("ASSET DOWNLOAD ERROR: " + ex.Message, false);
                }
            }

            DownloadHandle.Set();
        }
    }
}