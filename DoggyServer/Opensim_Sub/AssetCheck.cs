using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Opensim_Sub
{
    class AssetCheck
    {
        public static void getAKFEfiles(GridClient client)
        {
            foreach (string file in Directory.GetFiles("./"))
            {
                FileInfo fileInfo = new FileInfo(file);

                if (fileInfo.Extension.ToLower() == ".akfe") FromFile(client, file);
            }

        }

        public static void FromFile(GridClient client, string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Output_sub.Logs.add("REQUEST MESH ASSET: " + line, false);

                    byte[] assetData = Asset_Sub.Mesh.getAssetData(client, UUID.Parse(line));

                    if (assetData != null)
                    {
                        Output_sub.Logs.add("REQUESTED MESH ASSET: " + assetData.Count().ToString(), false);

                        Database_Sub.OpenSim.updateAssetData(UUID.Parse(line), assetData);
                    }
                    else Output_sub.Logs.add("REQUESTED MESH ASSET: " + line + " -> NULL", false);
                }
            }

            File.Delete(filename);
        }
    }
}
