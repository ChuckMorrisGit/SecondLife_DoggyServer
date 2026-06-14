using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub
{
    class Collision
    {
        public static void init(GridClient client)
        {
            client.Self.MeanCollision += new EventHandler<MeanCollisionEventArgs>(Self_MeanCollision);
            
        }

        static void Self_MeanCollision(object sender, MeanCollisionEventArgs e)
        {
            UUID key = e.Aggressor;

            string agressor = key.ToString();

            if (DoggyServer_main.avaDatas.ContainsKey(key)) agressor = DoggyServer_main.avaDatas[key].fullname;

            Output_sub.Logs.add("COLLISION WITH: " + agressor, false);
        }
    }
}
