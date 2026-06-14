using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Sit.init(client);
                Autopilot.Data.init(client);
                Autopilot.PosData.init();
                Target.init.all(client);
                Collision.init(client);
                NavMesh.init.all(client);
            }
            catch (Exception ex)
            {

                Output_sub.Logs.add("ERROR Movement_Sub" + ex.Message, false);

            }
        }
    }
}
