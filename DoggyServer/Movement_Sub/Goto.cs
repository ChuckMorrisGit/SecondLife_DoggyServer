using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub
{
    class Goto
    {
        public static void toAva(GridClient client, string avaName)
        {
            List<string> avasOnSim = DoggyServer_main.avasOnSim.Keys.ToList();

            foreach (string avaOnSim in avasOnSim)
            {
                if (avaOnSim.ToLower().Contains(avaName.ToLower()))
                {
                    ObjectManager_Sub.ObjectUpdate.followOn = false;
                    Move.to(client, DoggyServer_main.avasOnSim[avaOnSim].position);
                }
            }
        }

        public static void forward(GridClient client, UUID fromAV, float step)
        {
            if (step < 1) step = 5;
            if (fromAV!= UUID.Zero) client.Self.InstantMessage(fromAV, "go " + step.ToString() + "m forward");

            Vector3 distance = new Vector3(step, 0f, 0f);

            Vector3 newPos = client.Self.RelativePosition + distance * client.Self.RelativeRotation;

            Move.to(client, newPos);
        }

        public static void turnTo (GridClient client, UUID fromAV, Vector3 turn_vector)
        {
            Stand.ava(client);
            client.Self.Movement.TurnToward(turn_vector);
            client.Self.InstantMessage(fromAV, "look to point " + turn_vector.ToString());
        }

        public static void turnRight(GridClient client, UUID fromAV, float angle)
        {
            Stand.ava(client);
            client.Self.Movement.TurnRight = true;

        }
    }
}
