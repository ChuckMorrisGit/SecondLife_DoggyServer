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
    class Ani
    {
        private static string aniNC = string.Empty;

        private static AutoResetEvent DownloadHandle = new AutoResetEvent(false);
        private static string sub_Dir = "../Animations/";
        private static List<UUID> downloaded_ani = new List<UUID>();

        public static void init(GridClient client)
        {
            aniNC = string.Empty;
            if (!Directory.Exists(sub_Dir)) Directory.CreateDirectory(sub_Dir);
        }

        public static void saveAnis2NC(string filename)
        {
            StreamWriter sw = new StreamWriter(sub_Dir + filename);
            sw.WriteLine(aniNC);
            sw.Close();
        }

        private static string downloadFilename = "";
        public static void download(GridClient client, UUID uuid, string filename)
        {
            if (!downloaded_ani.Contains(uuid))
            {
                DownloadHandle.Reset();
                downloadFilename = filename;
                client.Assets.RequestAsset(uuid, AssetType.Animation, true, Assets_OnAssetReceived);
                downloaded_ani.Add(uuid);
                Output_sub.Logs.add("Try do download Animation: " + uuid, false);

                if (DownloadHandle.WaitOne(120 * 1000, false))
                {
                }
            }
        }

        private static void Assets_OnAssetReceived(AssetDownload transfer, Asset asset)
        {
            if (transfer.Success)
            {
                try
                {
                    string filename = sub_Dir + downloadFilename + asset.AssetID.ToString();

                    BinBVHAnimationReader bvh = new BinBVHAnimationReader(asset.AssetData);

                    
                    /// Ani2BVH Converter FEHLT

                    //File.WriteAllText(filename, bvh.ToString());

                    //File.WriteAllBytes(filename + ".animatn", asset.AssetData);

                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("ANIMATION RIP: " + ex.Message, false);
                }

                DownloadHandle.Set();
            }
        }
    }
}