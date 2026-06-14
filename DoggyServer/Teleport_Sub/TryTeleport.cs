using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Teleport_Sub
{
    class TryTeleport
    {
        public static void home(GridClient client, UUID fromAva)
        {


            while (!Simulator_Sub.Parcel.CheckIfOwn(client))
            {
                client.Self.InstantMessage(fromAva, "I am not home");
                Home.go(client);

                System.Threading.Thread.Sleep(20000);
            }

            client.Self.SendTeleportLure(fromAva);
        }
    }
}
