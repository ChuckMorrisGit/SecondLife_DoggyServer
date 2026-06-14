using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace DoggyServer.Appearance_Sub
{
    class Data
    {
        public enum appearanceStatus
        {
            neu = 1,
            inDatabase = 2,
            localFile = 3,
            localAndDatabase = 4,
            delete = 5,
        }

        public static Dictionary<UUID, AvatarAppearancePacket> Appearances = new Dictionary<UUID, AvatarAppearancePacket>();
        public static Dictionary<UUID, appearanceStatus> Appearance_status = new Dictionary<UUID,appearanceStatus>();

    }
}
