using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;
using System.Threading;

namespace DoggyServer.ObjectManager_Sub
{
    class ObjectUpdate
    {
        private static GridClient client;
        public static Boolean followOn = false;
        public static Boolean objectUpdate = false;
        public static string findPrimString = "";
        public static UUID findPrimFromAV = UUID.Zero;
        public static Dictionary<uint, Primitive> textureUpdates = new Dictionary<uint, Primitive>();
        public static Dictionary<uint, Primitive.SculptData> sculptData = new Dictionary<uint, Primitive.SculptData>();
        public static uint currentLocalID = 0;
        private static Dictionary<uint, Boolean> textureDownload = new Dictionary<uint, bool>();
        public static Dictionary<uint, Primitive> prim_properties = new Dictionary<uint, Primitive>();

        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Objects.TerseObjectUpdate += new EventHandler<TerseObjectUpdateEventArgs>(Objects_TerseObjectUpdate);
            client.Objects.ObjectProperties += new EventHandler<ObjectPropertiesEventArgs>(Objects_ObjectProperties);
            client.Objects.ObjectDataBlockUpdate += new EventHandler<ObjectDataBlockUpdateEventArgs>(Objects_ObjectDataBlockUpdate);
            client.Objects.ObjectUpdate += new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);

            AvaLogger.init(client);
        }

        public static void close(GridClient client_new)
        {
            client = client_new;
            client.Objects.TerseObjectUpdate -= new EventHandler<TerseObjectUpdateEventArgs>(Objects_TerseObjectUpdate);
            client.Objects.ObjectProperties -= new EventHandler<ObjectPropertiesEventArgs>(Objects_ObjectProperties);
            client.Objects.ObjectDataBlockUpdate -= new EventHandler<ObjectDataBlockUpdateEventArgs>(Objects_ObjectDataBlockUpdate);
            client.Objects.ObjectUpdate -= new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);

        }

        public static void clearData()
        {
            textureUpdates = new Dictionary<uint, Primitive>();
            sculptData = new Dictionary<uint, Primitive.SculptData>();
            textureDownload = new Dictionary<uint, bool>();
            prim_properties = new Dictionary<uint, Primitive>();
        }

        private static AutoResetEvent propertyDone = new AutoResetEvent(false);
        private static Primitive primProperty = new Primitive();
        private static Boolean download_texture = false;
        public static Primitive getProperty(GridClient client, uint localID, Boolean downloadTexture)
        {
            if (!textureDownload.Keys.Contains(localID)) textureDownload.Add(localID, downloadTexture);

            if (downloadTexture) Output_sub.Logs.add("TEXTURE DOWNLOAD -> Local ID: " + localID.ToString(), false);

            primProperty = new Primitive();
            propertyDone.Reset();

            download_texture = downloadTexture;
            currentLocalID = localID;
            client.Objects.SelectObject(client.Network.CurrentSim, localID);

            if (!propertyDone.WaitOne(10000, false))
            {
                Output_sub.Logs.add("GET PROPERTY FAILED: " + localID.ToString(), false);
                primProperty = null;
            }
            else
            {
                if (!Data.localPrimsNames.Keys.Contains(localID)) Data.localPrimsNames.Add(localID, primProperty.Properties.Name);
            }

            if ((sculptData.ContainsKey(localID)) && (download_texture))
            {
                primProperty.Sculpt = sculptData[localID];
                Output_sub.Logs.add("FOUND SCULPT DATA: " + primProperty.Sculpt.SculptTexture.ToString(), false);
            }

            

            return (primProperty);
        }

        static void Objects_ObjectUpdate(object sender, PrimEventArgs e)
        {
            if (e.Prim.Type == PrimType.Sculpt)
            {
                lock (sculptData)
                {
                    if (sculptData.ContainsKey(e.Prim.LocalID)) sculptData[e.Prim.LocalID] = e.Prim.Sculpt;
                    else sculptData.Add(e.Prim.LocalID, e.Prim.Sculpt);
                }

                //Output_sub.Logs.add("FOUND SCULPT: " + e.Prim.Sculpt.SculptTexture.ToString(), false);
                //Output_sub.Logs.add("@           : " + e.Prim.Position.ToString(), false);
            }

            
        }

        static void Objects_ObjectDataBlockUpdate(object sender, ObjectDataBlockUpdateEventArgs e)
        {
            if (e.Prim.Type == PrimType.Sculpt)
            {
                lock (sculptData)
                {
                    if (sculptData.ContainsKey(e.Prim.LocalID)) sculptData[e.Prim.LocalID] = e.Prim.Sculpt;
                    else sculptData.Add(e.Prim.LocalID, e.Prim.Sculpt);
                }

                //Output_sub.Logs.add("FOUND SCULPT: " + e.Prim.Sculpt.SculptTexture.ToString(), false);
                //Output_sub.Logs.add("@           : " + e.Prim.Position.ToString(), false);
            }
        }

        static void Objects_ObjectProperties(object sender, ObjectPropertiesEventArgs e)
        {
            Output_sub.Logs.add("GET PRIM PROPERTIES FOR: " + e.Properties.Name, false);

            try
            {
                primProperty.Properties = e.Properties;

                if (!prim_properties.Keys.Contains(currentLocalID)) prim_properties.Add(currentLocalID, primProperty);

            }
            catch (Exception ex) { Output_sub.Logs.add("PROPERTIES DICTIONARY FOR: " + e.Properties.Name + " ERROR: " + ex.Message, false); }

            propertyDone.Set();

            try
            {
                if (download_texture)
                {
                    foreach (UUID id in e.Properties.TextureIDs)
                    {
                        Output_sub.Logs.add("Prop ID: " + id.ToString(), false);

                        Asset_Sub.Image.get(client, "Prim", id);
                    }
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("TEXTURE DOWNLOAD ERROR: " + ex.Message, false); }

            try
            {
                string name = e.Properties.Name;
                if (name.ToLower().Contains(findPrimString.ToLower()))
                {
                    if (ObjectManager_Sub.FindObejct.PrimsWaiting.Keys.Contains(e.Properties.ObjectID))
                    {
                        name += ": " + ObjectManager_Sub.FindObejct.PrimsWaiting[e.Properties.ObjectID].Position.ToString();
                    }

                    if (findPrimFromAV != UUID.Zero) client.Self.InstantMessage(findPrimFromAV, name);
                }

                if (e.Properties.SalePrice == 0)
                {
                    Output_sub.Logs.add("Could Buy Prim: " + e.Properties.Name, false);

                }
            }
            catch (Exception ex) { Output_sub.Logs.add("PROPERTIES FOR: " + e.Properties.Name + " ERROR: " + ex.Message, false); }

        }

        static void Objects_TerseObjectUpdate(object sender, TerseObjectUpdateEventArgs e)
        {

            if (e.Update.Avatar)
            {
                Avatar av = new Avatar();

                client.Network.CurrentSim.ObjectsAvatars.TryGetValue(e.Update.LocalID, out av);
                if (av == null) return;

                if (DoggyServer_main.avasOnSim.Keys.Contains(av.Name))
                    if ((av.Position.X > 1) && (av.Position.Y > 1) && (av.Position.Z > 1))
                        DoggyServer_main.avasOnSim[av.Name].position = av.Position;

                float distance = Vector3.Distance(av.Position, client.Self.SimPosition);
                #region Master Actions
                if (av.Name == Master.currentMaster)
                {
                    Grid_Sub.FindMaster.masterLastFound = DateTime.Now;
                    if (followOn == true)
                    {

                        Movement_Sub.Follow.master(client, e.Update.Position, false);
                    }
                }
                #endregion

                //if ((distance < 19) && (Simulator_Sub.Parcel.CheckIfOwn(client)))
                if (distance < 19)
                    Communication_Sub.Greeter.checkAndGreet(client, av.Name, av.ID);

                if (Simulator_Sub.ParcelUpdate.simName == "Ruby Dust") AvaLogger.ckeckNlog(client, av);

                if (!DoggyServer_main.avaDatas.Keys.Contains(av.ID)) Avatars_Sub.NewAvatar.add(client, av);

            }
            else // Object ist Object....
            {
                try
                {
                    if (objectUpdate)
                    {
                        Primitive ob = new Primitive();
                        client.Network.CurrentSim.ObjectsPrimitives.TryGetValue(e.Update.LocalID, out ob);
                        if (ob == null) return;

                        GridSearch.ObjectByOwnerID(client, ob);

                        if (ob.Properties.Name.ToLower().Contains("devPose"))
                        {
                            Output_sub.Logs.add("OBJECT FOUND: " + ob.Position.ToString(), false);
                            FileStream greeterlogStream = new FileStream("./" + client.Self.LastName + "_ObjecktScanner.log", FileMode.Append);
                            StreamWriter greeterLogFile = new StreamWriter(greeterlogStream);

                            greeterLogFile.Write(DateTime.Now.ToString() + " " + ob.Properties.Name + " -> " + ob.Position.ToString());
                            greeterLogFile.WriteLine(" -> " + Simulator_Sub.ParcelUpdate.simName + " -> " + Simulator_Sub.ParcelUpdate.parcelName);

                            greeterLogFile.Close();

                        }

                        //if (ob.Text != "") Output_sub.Logs.add(ob.Text, false);
                    }
                }
                catch (Exception ex) { Output_sub.Logs.add("ERROR Object UPDATE: " + ex.Message, false); }
            }
        }
    }
}