using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.ObjectManager_Sub.Copy
{
    class Rez
    {
        private enum ImporterState
        {
            RezzingParent,
            RezzingChildren,
            Linking,
            Idle
        }

        private static ImporterState state = ImporterState.Idle;
        private static AutoResetEvent primDone = new AutoResetEvent(false);
        private static Primitive localRootPrim = new Primitive();
        private static Primitive currentPrim = new Primitive();
        private static Vector3 currentPosition = new Vector3();
        private static List<Primitive> localChildPrims = new List<Primitive>();
        private static GridClient aktuellerClient;
        private static Quaternion rootRotation = Quaternion.Identity;
        private static Dictionary<uint, uint> sourceTargetPrim = new Dictionary<uint, uint>();

        public static void init(GridClient client)
        {
            aktuellerClient = client;
            client.Objects.ObjectUpdate += new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);
        }

        public static void close(GridClient client)
        {
            aktuellerClient = client;
            client.Objects.ObjectUpdate -= new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);
        }

        public static void primSet(GridClient client, uint localID)
        {
            if (Data.linksets.ContainsKey(localID))
            {
                aktuellerClient = client;
                LinkSet linkset = Data.linksets[localID];
                Primitive rootPrim = updateProperty(client, linkset.RootPrim);

                Vector3 rootPrim_pos = client.Self.SimPosition;
                rootPrim_pos.Z += 3.0f;
                currentPosition = rootPrim_pos;

                //Output_sub.Logs.add("ROOT PRIM POS: " + rootPrim_pos.ToString(), false);

                state = ImporterState.RezzingParent;
                primDone.Reset();
                currentPrim = rootPrim;
                rootRotation = rootPrim.Rotation;
                rootPrim.Rotation = Quaternion.Identity;

                client.Objects.AddPrim(client.Network.CurrentSim, rootPrim.PrimData, client.Self.ActiveGroup, rootPrim_pos, rootPrim.Scale, rootPrim.Rotation);

                if (!primDone.WaitOne(10000, false)) Output_sub.Logs.add("FAILD REZZIN", false);

                state = ImporterState.RezzingChildren;
                foreach (Primitive prim in linkset.Children)
                {
                    Vector3 childPrim_pos = rootPrim_pos + prim.Position;
                    currentPosition = childPrim_pos;
                    //Output_sub.Logs.add("CHILD PRIM POS: " + childPrim_pos.ToString(), false);

                    primDone.Reset();
                    currentPrim = updateProperty(client, prim);
                    client.Objects.AddPrim(client.Network.CurrentSim, prim.PrimData, client.Self.ActiveGroup, childPrim_pos, prim.Scale, prim.Rotation);
                    if (!primDone.WaitOne(10000, false)) Output_sub.Logs.add("FAILD REZZIN", false);

                }

                /// Linken

                List<uint> linkPrims = new List<uint>();
                linkPrims.Add(localRootPrim.LocalID);
                foreach (Primitive prim in localChildPrims) linkPrims.Add(prim.LocalID);
                state = ImporterState.Linking;
                primDone.Reset();
                client.Objects.LinkPrims(client.Network.CurrentSim, linkPrims);
                if (!primDone.WaitOne(10000, false)) Output_sub.Logs.add("FAILD LINKING", false);

                state = ImporterState.Idle;

                client.Objects.SetRotation(client.Network.CurrentSim, rootPrim.LocalID, rootRotation);

                
            }
        }

        private static Primitive updateProperty(GridClient client, Primitive prim)
        {
            Primitive primProperty = ObjectManager_Sub.ObjectUpdate.getProperty(client, prim.LocalID,true);

            if (primProperty != null)
            {
                prim.Properties = primProperty.Properties;
                prim.Sculpt = primProperty.Sculpt;

                if (ObjectUpdate.sculptData.ContainsKey(prim.LocalID))
                {
                    Output_sub.Logs.add("UPDATE SCULPT DATA",false);

                    prim.Sculpt = ObjectUpdate.sculptData[prim.LocalID];

                    Asset_Sub.Image.get(client, "./sculptTexture/", prim.Sculpt.SculptTexture);
                    

                }
            }
            return (prim);
        }

        static void Objects_ObjectUpdate(object sender, PrimEventArgs e)
        {
            Primitive prim = e.Prim;

            //if (prim.Text!="")  Output_sub.Logs.add(prim.Text, false);

            if ((prim.Flags & PrimFlags.CreateSelected) != 0)
            {
                //Output_sub.Logs.add("ObjectUpdate from: " + prim.OwnerID.ToString() + " -> " + aktuellerClient.Self.AgentID.ToString(), false);
                switch (state)
                {
                    case ImporterState.RezzingParent:
                        localRootPrim = prim;
                        UpdateRezzedPrim(prim, e);
                        break;

                    case ImporterState.RezzingChildren:
                        localChildPrims.Add(prim);
                        UpdateRezzedPrim(prim, e);
                        break;
                }

                primDone.Set();
                try { sourceTargetPrim.Add(currentPrim.LocalID, prim.LocalID); }
                catch (Exception ex) { Output_sub.Logs.add("REZZER TEXT UPDATE: " + ex.Message, false); }
            }
        }

        private static void UpdateRezzedPrim(Primitive prim, PrimEventArgs e)
        {
            try
            {

                try
                {
                    aktuellerClient.Objects.SetTextures(aktuellerClient.Network.CurrentSim, prim.LocalID, currentPrim.Textures);
                }
                catch (Exception ex) { Output_sub.Logs.add("UpdateRezzedPrim Texture: " + ex.Message, false); }

                aktuellerClient.Objects.SetPosition(e.Simulator, prim.LocalID, currentPosition);

                if (currentPrim.Light.Intensity > 0)
                {
                    aktuellerClient.Objects.SetLight(e.Simulator, prim.LocalID, currentPrim.Light);
                }

                aktuellerClient.Objects.SetFlexible(e.Simulator, prim.LocalID, currentPrim.Flexible);

                if (currentPrim.Sculpt.SculptTexture != UUID.Zero)
                {
                    aktuellerClient.Objects.SetSculpt(e.Simulator, prim.LocalID, currentPrim.Sculpt);
                }
                else
                {

                }

                if (!String.IsNullOrEmpty(currentPrim.Properties.Name))
                    aktuellerClient.Objects.SetName(e.Simulator, prim.LocalID, currentPrim.Properties.Name);
                if (!String.IsNullOrEmpty(currentPrim.Properties.Description))
                    aktuellerClient.Objects.SetDescription(e.Simulator, prim.LocalID, currentPrim.Properties.Description);
            }
            catch (Exception ex) { Output_sub.Logs.add("UpdateRezzedPrim: " + ex.Message, false); }
        }
    }
}
