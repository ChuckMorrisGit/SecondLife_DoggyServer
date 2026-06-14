using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.IO;
using System.Xml;

namespace DoggyServer.Avatars_Sub
{
    class View
    {
        public static Dictionary<UUID, bool> m_AgentList = new Dictionary<UUID, bool>();
        public static Dictionary<UUID, List<UUID>> m_AgentList_ViewedObjects = new Dictionary<UUID, List<UUID>>();
        public static Boolean autoRipper = true;
        public static Boolean autoRipper_prims = true;

        public static string subDir_save = "./Prims/";


        public static void init(GridClient client)
        {
            client.Avatars.ViewerEffect += new EventHandler<ViewerEffectEventArgs>(Avatars_ViewerEffect);
            client.Avatars.ViewerEffectLookAt += new EventHandler<ViewerEffectLookAtEventArgs>(Avatars_ViewerEffectLookAt);
            client.Avatars.ViewerEffectPointAt += new EventHandler<ViewerEffectPointAtEventArgs>(Avatars_ViewerEffectPointAt);
        }

        public static void close(GridClient client)
        {
            client.Avatars.ViewerEffect -= new EventHandler<ViewerEffectEventArgs>(Avatars_ViewerEffect);
            client.Avatars.ViewerEffectLookAt -= new EventHandler<ViewerEffectLookAtEventArgs>(Avatars_ViewerEffectLookAt);
            client.Avatars.ViewerEffectPointAt -= new EventHandler<ViewerEffectPointAtEventArgs>(Avatars_ViewerEffectPointAt);
        }

        private static void Avatars_ViewerEffectPointAt(object sender, ViewerEffectPointAtEventArgs e)
        {
            GridClient client = e.Simulator.Client;

            UUID sourceID = e.SourceID;
            UUID targetID = e.TargetID;

            if (DoggyServer_main.avaDatas.ContainsKey(sourceID))
            {
                DoggyServer_main.avaDatas[e.SourceID].pointAt = e.TargetPosition;
                DoggyServer_main.avaDatas[e.SourceID].pointAt_id = e.TargetID;
            }

            lock (m_AgentList)
            {
                if (m_AgentList.ContainsKey(sourceID))
                    m_AgentList[sourceID] = true;
                else
                    m_AgentList.Add(sourceID, true);
            }

            if ((autoRipper) && (Master.firstLevelMaster.Contains(sourceID)))
            {
                List<UUID> prims = new List<UUID>();
                if (!m_AgentList_ViewedObjects.ContainsKey(sourceID)) m_AgentList_ViewedObjects.Add(sourceID, prims);

                prims = m_AgentList_ViewedObjects[sourceID];
                if (!prims.Contains(targetID))
                {
                    prims.Add(targetID);
                    m_AgentList_ViewedObjects[e.SourceID] = prims;

                    lock (client.Network.Simulators)
                    {
                        for (int i = 0; i < client.Network.Simulators.Count; i++)
                        {
                            try
                            {
                                client.Network.Simulators[i].ObjectsPrimitives.ForEach(
                                    delegate(Primitive prim)
                                    {
                                        if (prim != null)
                                        {
                                            
                                            if (prim.ID == e.TargetID)
                                            {
                                                ObjectManager_Sub.Prim.get(client, prim.LocalID);
                                                try
                                                {
                                                    if (autoRipper_prims)
                                                    {
                                                        string filename = subDir_save + prim.LocalID.ToString() + ".xml";
                                                        if (!Directory.Exists(subDir_save)) Directory.CreateDirectory(subDir_save);
                                                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(prim.PrimData.GetType());
                                                        TextWriter tw = new StreamWriter(filename);
                                                        x.Serialize(tw, prim.PrimData);
                                                        tw.WriteLine("Prim Type: " + prim.Type.ToString());
                                                        tw.Close();
                                                    }
                                                }
                                                catch (Exception ex) { Output_sub.Logs.add("ERROR Prim Properties: " + ex.Message, false); }
                                            }
                                        }
                                    }
                               );
                            }
                            catch (Exception ex) { Output_sub.Logs.add("View -> Primabfrage: " + ex.Message, false); }
                        }
                    }
                }
            }
        }

        private static void Avatars_ViewerEffectLookAt(object sender, ViewerEffectLookAtEventArgs e)
        {
            if (DoggyServer_main.avaDatas.ContainsKey(e.SourceID))
            {
                DoggyServer_main.avaDatas[e.SourceID].lookAt = e.TargetPosition;
                DoggyServer_main.avaDatas[e.SourceID].lookAt_id = e.TargetID;
            }

            lock (m_AgentList)
            {
                if (m_AgentList.ContainsKey(e.SourceID))
                    m_AgentList[e.SourceID] = true;
                else
                    m_AgentList.Add(e.SourceID, true);
            }
        }

        private static void Avatars_ViewerEffect(object sender, ViewerEffectEventArgs e)
        {
            lock (m_AgentList)
            {
                if (m_AgentList.ContainsKey(e.SourceID))
                    m_AgentList[e.SourceID] = true;
                else
                    m_AgentList.Add(e.SourceID, true);
            }
        }

    }
}
