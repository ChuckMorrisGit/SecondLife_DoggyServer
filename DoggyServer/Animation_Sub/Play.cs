using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Animation_Sub
{
    class Play
    {
        public static void byUUID(GridClient client, UUID ani)
        {
            if ((Simulator_Sub.ParcelUpdate.flags & ParcelFlags.AllowOtherScripts) == ParcelFlags.AllowOtherScripts)
            {
                Dictionary<UUID, Boolean> on = new Dictionary<UUID, bool>();
                Dictionary<UUID, Boolean> off = new Dictionary<UUID, bool>();

                on.Add(ani, true);
                off.Add(ani, false);

                Set.off(client);
                client.Self.Animate(on, false);
                System.Threading.Thread.Sleep(10000);

                client.Self.Animate(off, false);
                Set.on(client);
            }
        }

        public static void doggyClean(GridClient client)
        {
            UUID ani = new UUID("2609d19b-0b5e-2ff7-79cf-d2f38775ec6d");
            Movement_Sub.Stand.ava(client);
            byUUID(client, ani);
        }
    }
}
