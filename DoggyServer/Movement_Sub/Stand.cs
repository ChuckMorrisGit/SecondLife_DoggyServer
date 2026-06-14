using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub
{
    class Stand
    {
        public static void ava(GridClient client)
        {
            Animation_Sub.AO.ResetAutoAnimation();
            client.Self.Stand();

            /*
            client.Self.SignaledAnimations.ForEach(delegate(KeyValuePair<UUID, int> kvp)
            {
                client.Self.AnimationStop(kvp.Key, true);
            });
             */ 
        }
    }
}
