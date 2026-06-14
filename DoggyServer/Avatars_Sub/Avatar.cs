using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Avatars_Sub
{
    class Avatar
    {
        private static GridClient client;
        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Objects.AvatarUpdate += new EventHandler<AvatarUpdateEventArgs>(Objects_AvatarUpdate);
        }

        public static void close(GridClient client_new)
        {
            client.Objects.AvatarUpdate -= new EventHandler<AvatarUpdateEventArgs>(Objects_AvatarUpdate);
        }

        static void Objects_AvatarUpdate(object sender, AvatarUpdateEventArgs e)
        {
            try
            {
                OpenMetaverse.Avatar av = e.Avatar;

                if (!DoggyServer_main.avaDatas.Keys.Contains(av.ID)) NewAvatar.add(client, av);

                DoggyServer_main.avaDatas[av.ID].position = av.Position;

                if (!DoggyServer_main.avasOnSim.Keys.Contains(av.Name))
                {
                    DoggyServer_main.avasOnSim.Add(av.Name, DoggyServer_main.avaDatas[av.ID]);
                }

                if ((av.Position.X > 1) && (av.Position.Y > 1) && (av.Position.Z > 1))
                {
                    DoggyServer_main.avasOnSim[av.Name].position = av.Position;
                    //Output_sub.Logs.add("Ava Pos: " + av.Name + " -> " + av.Position.ToString(), false);
                }

                
                
            }
            catch (Exception ex) { Output_sub.Logs.add("AvatarUpdate: " + ex.Message, false); }
        }
    }
}
