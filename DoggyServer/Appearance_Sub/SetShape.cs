using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace DoggyServer.Appearance_Sub
{
    class SetShape
    {
        private static uint SerialNum = 2;
        public static void byAvaKey(GridClient client, UUID avaKey, Boolean keepTexture)
        {
            AvatarAppearancePacket appearance = new AvatarAppearancePacket();
            if (Appearance_Sub.Data.Appearances.ContainsKey(avaKey))
            {
                appearance = Appearance_Sub.Data.Appearances[avaKey];

                direkt(client, appearance, keepTexture);
            }
        }

        public static string byAvaName(GridClient client, string name, Boolean keepTexture)
        {
            string avaName = "NONE";
            AvatarAppearancePacket appearance = new AvatarAppearancePacket();
            appearance = Shape.searchAndLoad(client, name);
            if (appearance != null)
            {
                direkt(client, appearance, keepTexture);
                avaName = DoggyServer_main.avaDatas[appearance.Sender.ID].fullname;
            }
            return (avaName);
        }

        public static void direkt(GridClient client, AvatarAppearancePacket appearance, Boolean keepTexture)
        {
            AgentSetAppearancePacket set = new AgentSetAppearancePacket();
            set.AgentData.AgentID = client.Self.AgentID;
            set.AgentData.SessionID = client.Self.SessionID;
            set.AgentData.SerialNum = SerialNum++;
            set.AgentData.Size = new Vector3(2f, 2f, 2f); // HACK

            set.WearableData = new AgentSetAppearancePacket.WearableDataBlock[0];
            set.VisualParam = new AgentSetAppearancePacket.VisualParamBlock[appearance.VisualParam.Length];

            for (int i = 0; i < appearance.VisualParam.Length; i++)
            {
                set.VisualParam[i] = new AgentSetAppearancePacket.VisualParamBlock();
                set.VisualParam[i].ParamValue = appearance.VisualParam[i].ParamValue;
                if (Debug.on) client.Self.Chat(appearance.VisualParam[i].ParamValue.ToString(), 0, ChatType.Normal);
            }

            if (keepTexture)
            {
                for (int i = 0; i < client.Network.Simulators.Count; i++)
                {
                    Avatar av = client.Network.Simulators[i].ObjectsAvatars.Find(
                        delegate(Avatar avatar)
                        {
                            return avatar.ID == client.Self.AgentID;
                        }
                    );

                    set.ObjectData.TextureEntry = av.Textures.GetBytes();
                }
            }
            else
            {
                set.ObjectData.TextureEntry = appearance.ObjectData.TextureEntry;
            }

            if (Debug.on) client.Self.Chat(set.ObjectData.TextureEntry.ToString(), 0, ChatType.Normal);
            if (Debug.on) client.Self.Chat("count:" + set.ObjectData.TextureEntry.Count().ToString(), 0, ChatType.Normal);

            Nude.prims(client, UUID.Zero);

            client.Network.SendPacket(set);
        }
    }
}
