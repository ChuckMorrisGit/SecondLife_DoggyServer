using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Simulator_Sub
{
    class SimUpdate
    {
        private static GridClient client;


        public static void init(GridClient client_new)
        {
            try
            {
                client = client_new;

                client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_SimChanged);
                client.Network.EventQueueRunning += new EventHandler<EventQueueRunningEventArgs>(Network_EventQueueRunning);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Simulator_Sub->SimUpdate: " + ex.Message, false);
            }
        }

        static void Network_EventQueueRunning(object sender, EventQueueRunningEventArgs e)
        {
            if (e.Simulator == client.Network.CurrentSim)
            {
                //client.Appearance.SetPreviousAppearance(true);
            }
        }

        static void Network_SimChanged(object sender, SimChangedEventArgs e)
        {
            if (e.PreviousSimulator != null)
            {
                
            }

            Group_Sub.Set.normal(client);
            ObjectManager_Sub.Copy.Data.linksets = new Dictionary<uint, ObjectManager_Sub.Copy.LinkSet>();
            //HUD_Sub.Scan.huds(client);
        }
    }
}
