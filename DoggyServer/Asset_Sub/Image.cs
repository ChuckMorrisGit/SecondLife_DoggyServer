using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenMetaverse;
using OpenMetaverse.Assets;
using FreeImageAPI;
using OpenMetaverse.Imaging;
//using convert2jpg;
using Paloma;

namespace DoggyServer.Asset_Sub
{
    class Image
    {
        public static string subDir = "./rip/";
        public static string subDir_jpg = "./rip_jpg/";
        //public static string dir_NextCloud = "/srv/shares/Clouds/NextCloud/SecondLife/";
        private static Dictionary<UUID, string> textures = new Dictionary<UUID, string>();

        public static void init(GridClient client)
        {
            //dir_NextCloud = getCloudRootFolder.Dropbox_Sub.DropboxRootFolder.get() + "/SecondLife/"; 

            //if (!Directory.Exists(dir_NextCloud)) Directory.CreateDirectory(dir_NextCloud);
            if (!Directory.Exists(subDir)) Directory.CreateDirectory(subDir);
            if (!Directory.Exists(subDir_jpg)) Directory.CreateDirectory(subDir_jpg);

            string[] files = Directory.GetFiles(subDir);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if ((DateTime.Now - fileInfo.CreationTime).TotalDays > 3) File.Delete(file);
            }

            client.Assets.ImageReceiveProgress += new EventHandler<ImageReceiveProgressEventArgs>(Assets_ImageReceiveProgress);

            deleteOldFiles(subDir);
            deleteOldFiles(subDir_jpg);
            //deleteOldFiles(dir_NextCloud);
        }

        private static void deleteOldFiles(string subDir_temp)
        {
            Output_sub.Logs.add("Delete Old Files in: " + subDir_temp, false);
            string[] files = Directory.GetFiles(subDir_temp);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if ((DateTime.Now - fileInfo.CreationTime).TotalDays > 30) File.Delete(file);
            }
        }

        public static void close(GridClient client)
        {
            client.Assets.ImageReceiveProgress -= new EventHandler<ImageReceiveProgressEventArgs>(Assets_ImageReceiveProgress);
        }

        public static void get(GridClient client, string subDir, UUID uuID)
        {
            try
            {
                if (!textures.Keys.Contains(uuID))
                {
                    textures.Add(uuID, subDir);
                    client.Assets.RequestImage(uuID, Assets_OnImageReceived);


                }
            }
            catch (Exception ex) { Output_sub.Logs.add("GET_IMAGE ERROR: " + ex.Message, false); }
        }

        public static void getServerBaked(GridClient client, string subDir, UUID uuID, UUID avaid, string bakeName)
        {
            try
            {
                if (!textures.Keys.Contains(uuID))
                {
                    textures.Add(uuID, subDir);
                    client.Assets.RequestServerBakedImage(avaid, uuID, bakeName, Assets_OnImageReceived);
                    //client.Assets.RequestImage(uuID, OpenMetaverse.ImageType.ServerBaked ,Assets_OnImageReceived);
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("GET_SERVER_BAKED ERROR: " + ex.Message, false); }
        }

        static void Assets_ImageReceiveProgress(object sender, ImageReceiveProgressEventArgs e)
        {
            //Output_sub.Logs.add("IMAGE REVEIVED: " + e.ImageID.ToString(), false);
        }

        private static void Assets_OnImageReceived(TextureRequestState state, AssetTexture asset)
        {

            string id = string.Empty;
            try
            {
                if (asset != null) id = asset.AssetID.ToString();
            }
            catch (Exception ex) { Output_sub.Logs.add("ASSET STATE ERROR: " + ex.Message, false); }

            Output_sub.Logs.add("ASSET STATE: " + state.ToString() + " " + id, false);

            try
            {
                if (state == TextureRequestState.Finished)
                {
                    if ((textures.Count > 0) && (textures.ContainsKey(asset.AssetID)))
                    {
                        string jp2Filename = subDir + textures[asset.AssetID] + "_" + asset.AssetID + ".jp2";
                        string jpgFilename = subDir_jpg + textures[asset.AssetID] + "_" + asset.AssetID + ".jpg";
                        string tgaFilename = subDir + textures[asset.AssetID] + "_" + asset.AssetID + ".tga";
                        string pngFilename = subDir + textures[asset.AssetID] + "_" + asset.AssetID + ".png";

                        //File.WriteAllBytes(jp2Filename, asset.AssetData);
                        //Output_sub.Logs.add("Texture downloaded: " + asset.AssetID.ToString(), false);

                        ManagedImage imgData;
                        OpenJPEG.DecodeToImage(asset.AssetData, out imgData);
                        

                        byte[] tgaFile = imgData.ExportTGA();

                        File.WriteAllBytes(tgaFilename, tgaFile);

                        Output_sub.Logs.add("Texture downloaded: " + textures[asset.AssetID] + " <-> " + asset.AssetID.ToString(), false);

                        //if(DoggyServer_main.platformID == PlatformID.Unix) convert2jpg.Convert_Sub.Convert.file(tgaFilename);

                        System.Drawing.Bitmap pic = Paloma.TargaImage.LoadTargaImage(tgaFilename);
                        pic.Save(pngFilename);

                        /*
                        string[] asset_owners = { "Mysty Miles", "Doggy Ghostraven", "Ulrike Lachman" };
                        if (asset_owners.Contains(textures[asset.AssetID]))
                        {
                            if (!Directory.Exists(dir_NextCloud)) Directory.CreateDirectory(dir_NextCloud);
                            string dir_dropbox_sub = (dir_NextCloud + textures[asset.AssetID] + "/").Replace(' ', '_');
                            if (!Directory.Exists(dir_dropbox_sub))
                            {
                                Directory.CreateDirectory(dir_dropbox_sub);
                            }

                            pngFilename = (dir_dropbox_sub + textures[asset.AssetID] + "_" + asset.AssetID + ".png").Replace(' ', '_');
                            pic.Save(pngFilename);
                            Output_sub.Logs.add(textures[asset.AssetID] + " Texture downloaded: " + pngFilename, false);
                            checkDropbox(dir_dropbox_sub);
                        }
                        */


                    }
                    else Output_sub.Logs.add("ASSET ERROR -> don't contains: " + asset.AssetID.ToString(), false);
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("ASSET IMAGE ERROR: " + ex.ToString(), false); }
        }

        public static void checkDropbox(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    TimeSpan span = DateTime.Now - fileInfo.CreationTime;
                    if (span.TotalMinutes > 60) File.Delete(file);
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("DROPBOX ERROR: " + ex.Message, false); }

        }
    }
}