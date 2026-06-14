using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace DoggyServer.Appearance_Sub
{
    class Manager
    {
        public static void init(GridClient client)
        {
            client.Network.RegisterCallback(PacketType.AvatarAppearance, AvatarAppearanceHandler);
            client.Appearance.AppearanceSet += new EventHandler<AppearanceSetEventArgs>(Appearance_AppearanceSet);
        }

        public static void close(GridClient client)
        {
            //client.Network.RegisterCallback(PacketType.AvatarAppearance, AvatarAppearanceHandler);
            client.Appearance.AppearanceSet -= new EventHandler<AppearanceSetEventArgs>(Appearance_AppearanceSet);
        }

        static void Appearance_AppearanceSet(object sender, AppearanceSetEventArgs e)
        {
            
        }

        private static void AvatarAppearanceHandler(object sender, PacketReceivedEventArgs e)
        {
            Packet packet = e.Packet;

            AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;

            lock (Data.Appearances)
            {
                Data.Appearances[appearance.Sender.ID] = appearance;
                Data.Appearance_status[appearance.Sender.ID] = Data.appearanceStatus.neu;
            }

            Output_sub.Logs.add("Add Appearance: " + DoggyServer_main.avaDatas[appearance.Sender.ID].fullname, false);

        }
    }
}
