using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub
{
    class Sit
    {
        public static void init(GridClient client)
        {
            client.Self.AvatarSitResponse += new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
        }

        static void Self_AvatarSitResponse(object sender, AvatarSitResponseEventArgs e)
        {
            
        }

        public static void onLocalPrim(GridClient client, UUID uuid)
        {

            client.Self.Stand();
            client.Self.RequestSit(uuid, Vector3.Zero);
            client.Self.Sit();
            Animation_Sub.AO.ResetAutoAnimation();

        }
    }
}
