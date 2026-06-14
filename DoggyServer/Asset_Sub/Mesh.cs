using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Assets;
using System.Threading;

namespace DoggyServer.Asset_Sub
{
    class Mesh
    {
        public static void init(GridClient client)
        {


        }

        public static byte[] getAssetData(GridClient client, UUID meshUUID)
        {
            byte[] assetData = null;

            try
            {
                AutoResetEvent gotMesh = new AutoResetEvent(false);

                client.Assets.RequestMesh(meshUUID, (success, meshAsset) =>
                    {
                        if (!success)
                        {
                            Output_sub.Logs.add("Failed to fetch or decode the mesh asset", false);
                        }
                        else
                        {
                            assetData = meshAsset.AssetData;
                        }

                        gotMesh.Set();
                    });

                if (!gotMesh.WaitOne(20 * 1000, false)) return (null);
            }
            catch (Exception ex) { Output_sub.Logs.add("ERROR MESH ASSET: " + ex.Message, false); }

            return (assetData);
        }

    }
}
