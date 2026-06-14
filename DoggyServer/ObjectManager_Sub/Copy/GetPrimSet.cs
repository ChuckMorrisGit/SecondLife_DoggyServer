using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;

namespace DoggyServer.ObjectManager_Sub.Copy
{
    class GetPrimSet
    {
        private static UUID mystyKey = new UUID("b7d6de4b-8588-4ea4-9255-6f035a00d510");
        private static string sub_Dir = "../PrimSets/";


        public static void whole(GridClient client, UUID fromAva, Boolean save_primset)
        {
            try
            {
                List<uint> localPrims = new List<uint>();
                uint rootPrim = 0;
                UUID viewObject_uuid = DoggyServer_main.avaDatas[fromAva].pointAt_id;
                List<Primitive> prims = new List<Primitive>();
                Output_sub.Logs.add("FIND Rootprim: ", false);

                /*
                lock (client.Network.Simulators)
                {
                    for (int i = 0; i < client.Network.Simulators.Count; i++)
                    {
                        client.Network.Simulators[i].ObjectsAvatars.ForEach(
                            delegate(Avatar av)
                            {
                                if (av.ID == mystyKey) rootPrim = av.LocalID;
                            }
                       );
                    }
                }
                 */

                lock (client.Network.Simulators)
                {
                    for (int i = 0; i < client.Network.Simulators.Count; i++)
                    {
                        client.Network.Simulators[i].ObjectsPrimitives.ForEach(
                            delegate(Primitive prim)
                            {
                                //Temporärer Versuch
                                client.Objects.SelectObject(client.Network.CurrentSim, prim.LocalID);

                                prims.Add(prim);
                                if ((viewObject_uuid == prim.ID) && (prim.ParentID == 0))
                                {
                                    rootPrim = prim.LocalID;
                                    Output_sub.Logs.add("ADD Rootprim: " + prim.LocalID.ToString(), false);
                                }
                            }
                       );
                    }
                }


                if (rootPrim != 0)
                {
                    Primitive prim;

                    for (int i = 0; i < prims.Count; i++)
                    {
                        prim = prims[i];

                        if (prim.ParentID == 0)
                        {
                            if (Data.linksets.ContainsKey(prim.LocalID))
                                Data.linksets[prim.LocalID].RootPrim = prim;
                            else
                                Data.linksets[prim.LocalID] = new LinkSet(prim);
                        }
                        else
                        {
                            if (!Data.linksets.ContainsKey(prim.ParentID))
                                Data.linksets[prim.ParentID] = new LinkSet();

                            Data.linksets[prim.ParentID].Children.Add(prim);
                        }

                        if (prim.ParentID == rootPrim) Output_sub.Logs.add("ADD Child Prim: " + prim.LocalID.ToString(), false);
                    }


                    /// SAve PrimSets
                    if (save_primset)
                    {
                        if (!Directory.Exists(sub_Dir)) Directory.CreateDirectory(sub_Dir);


                        //Data.linksets[prim.ParentID].

                    }
                }

                Output_sub.Logs.add("Scanned Linksets: " + Data.linksets.Count().ToString(), false);


                Rez.primSet(client, rootPrim);
            }
            catch (Exception ex) { Output_sub.Logs.add("GetPrims ERROR: " + ex.Message, false); }
        }
    }
}