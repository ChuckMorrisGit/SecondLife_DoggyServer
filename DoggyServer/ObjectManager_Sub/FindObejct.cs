using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;
using System.Threading;

namespace DoggyServer.ObjectManager_Sub
{
    class FindObejct
    {
        public static Dictionary<UUID, Primitive> PrimsWaiting = new Dictionary<UUID, Primitive>();
        //AutoResetEvent AllPropertiesReceived = new AutoResetEvent(false);
        private static GridClient client;

        public static List<Primitive> inRange(GridClient client, int range)
        {
            Vector3 location = client.Self.SimPosition;

            List<Primitive> prims_return = new List<Primitive>();

            List<uint> localIDs = new List<uint>();

            client.Network.CurrentSim.ObjectsPrimitives.ForEach(
                   delegate(Primitive prim)
                   {
                       Vector3 pos = prim.Position;
                       if (Vector3.Distance(pos, location) < range)
                       {
                           prims_return.Add(prim);
                           if (!localIDs.Contains(prim.ParentID)) localIDs.Add(prim.LocalID);
                       }

                       if (localIDs.Contains(prim.ParentID))
                       {
                           prims_return.Add(prim);
                           if (!localIDs.Contains(prim.ParentID)) localIDs.Add(prim.LocalID);
                       }

                       //if (prim.ParentID != 0) Output_sub.Logs.add("FOUND CHILD FROM: " + prim.ParentID.ToString(), false);
                   }
            );

            //bool complete = RequestObjectProperties(prims_return, 250);
            return (prims_return);
        }

        public static void perName(GridClient client_new, string name, UUID fromAV)
        {
            client = client_new;
            ObjectManager_Sub.ObjectUpdate.findPrimString = name.TrimEnd(' ');
            ObjectManager_Sub.ObjectUpdate.findPrimFromAV = fromAV;

            if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Search for: >" + ObjectManager_Sub.ObjectUpdate.findPrimString + "<");

            List<Primitive> prims = new List<Primitive>();
            try
            {
                Vector3 location = client.Self.SimPosition;

                // *** find all objects in radius ***
                prims = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                    delegate(Primitive prim)
                    {
                        Vector3 pos = prim.Position;
                        //return ((prim.ParentID == 0) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < radius));
                        return ((prim.ParentID == 0) && (pos != Vector3.Zero));
                    }
                );
            }
            catch (Exception ex) { Output_sub.Logs.add("FindAll Prim ERROR: " + ex.Message, false); }

            bool complete = RequestObjectProperties(prims, 250);

            try
            {
                FileStream greeterlogStream = new FileStream("./" + client.Self.LastName + "_ObjecktScanner.log", FileMode.Append);
                StreamWriter greeterLogFile = new StreamWriter(greeterlogStream);
                foreach (Primitive prim in prims)
                {
                    Output_sub.Logs.add("OBJECT FOUND: " + prim.Position.ToString(), false);
                    greeterLogFile.Write(DateTime.Now.ToString() + " " + prim.Properties.Name + " -> " + prim.Position.ToString());
                    greeterLogFile.WriteLine(" -> " + Simulator_Sub.ParcelUpdate.simName + " -> " + Simulator_Sub.ParcelUpdate.parcelName);
                }
                greeterLogFile.Close();
            }
            catch (Exception ex) { Output_sub.Logs.add("Search Prim ERROR: " + ex.Message, false); }
        }

        private static bool RequestObjectProperties(List<Primitive> objects, int msPerRequest)
        {
            // Create an array of the local IDs of all the prims we are requesting properties for
            uint[] localids = new uint[objects.Count];

            lock (PrimsWaiting)
            {
                PrimsWaiting.Clear();

                for (int i = 0; i < objects.Count; ++i)
                {
                    localids[i] = objects[i].LocalID;
                    PrimsWaiting.Add(objects[i].ID, objects[i]);
                }
            }

            client.Objects.SelectObjects(client.Network.CurrentSim, localids);

            //return AllPropertiesReceived.WaitOne(2000 + msPerRequest * objects.Count, false);
            return (true);
        }

        void Objects_OnObjectProperties(object sender, ObjectPropertiesEventArgs e)
        {
            lock (PrimsWaiting)
            {
                Primitive prim;
                if (PrimsWaiting.TryGetValue(e.Properties.ObjectID, out prim))
                {
                    prim.Properties = e.Properties;
                }
                PrimsWaiting.Remove(e.Properties.ObjectID);

                //if (PrimsWaiting.Count == 0) AllPropertiesReceived.Set();
            }
        }

    }
}
