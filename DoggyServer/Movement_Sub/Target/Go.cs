using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub.Target
{
    class Go
    {
        public static void to(GridClient client, string location, UUID fromAva)
        {
            if (Data.waypoints.ContainsKey(location))
            {

                if (Teleport_Sub.Teleport.toLM_UUID(client, Data.teleKey[location]))
                {
                    client.Self.InstantMessage(fromAva, "Teleported to " + client.Network.CurrentSim.Name);

                    foreach (Vector3 point in Data.waypoints[location])
                    {
                        while (Vector3.Distance(point, client.Self.SimPosition) > 2)
                        {
                            Movement_Sub.Move.to(client, point);
                            System.Threading.Thread.Sleep(1000);
                        }
                        client.Self.InstantMessage(fromAva, "Reached Waypount: " + point.ToString());
                    }

                    client.Self.SendTeleportLure(fromAva);
                }
            }
        }
    }
}
