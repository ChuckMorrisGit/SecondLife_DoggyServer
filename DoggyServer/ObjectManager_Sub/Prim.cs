using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.ObjectManager_Sub
{
    class Prim
    {
        //public static AutoResetEvent primPropertiesDone = new AutoResetEvent(false);
        public static void get(GridClient client, uint localID)
        {
            //primPropertiesDone.Reset();
            //ObjectUpdate.currentLocalID = localID;

            
            ObjectUpdate.getProperty(client, localID, true);
            //client.Objects.SelectObject(client.Network.CurrentSim, localID);

            //primPropertiesDone.WaitOne(10000);
        }

        public static void copyLocalTextures(GridClient client, uint sourcePrim, uint targetPrim, Primitive prim)
        {
            Output_sub.Logs.add("Update: " + targetPrim.ToString(), false);
            ObjectManager_Sub.ObjectUpdate.textureUpdates.Add(sourcePrim, prim);
            ObjectManager_Sub.Prim.get(client, sourcePrim);

        }
    }
}
