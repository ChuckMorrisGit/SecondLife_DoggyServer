using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Http;
using System.IO;

namespace DoggyServer.Avatars_Sub
{
    class Appearance
    {
        private static GridClient client;
        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Avatars.AvatarAppearance += new EventHandler<AvatarAppearanceEventArgs>(Avatars_AvatarAppearance);
        }

        public static void close(GridClient client_new)
        {
            client = client_new;
            client.Avatars.AvatarAppearance -= new EventHandler<AvatarAppearanceEventArgs>(Avatars_AvatarAppearance);
        }

        static void Avatars_AvatarAppearance(object sender, AvatarAppearanceEventArgs e)
        {
            try
            {
                Primitive.TextureEntryFace[] textures = e.FaceTextures;


                int face = 0;
                foreach (Primitive.TextureEntryFace texture in textures)
                {
                    if ((texture != null) && (texture.TextureID != UUID.Zero))
                    {
                        AvatarTextureIndex type = (AvatarTextureIndex)face;

                        string ava_name = e.AvatarID.ToString();
                        if (DoggyServer_main.avaDatas.ContainsKey(e.AvatarID)) ava_name = DoggyServer_main.avaDatas[e.AvatarID].fullname;

                        try
                        {
                            if (DoggyServer_main.avaripMode)
                            {

                                switch (type)
                                {
                                    case AvatarTextureIndex.HeadBaked:
                                        Output_sub.Logs.add("Ava Texture:" + ava_name + "  F: " + type.ToString() + "  U: " + texture.TextureID.ToString(), false);
                                        Asset_Sub.Image.getServerBaked(client, DoggyServer_main.avaDatas[e.AvatarID].fullname, texture.TextureID, e.AvatarID, "head");
                                        break;

                                    case AvatarTextureIndex.UpperBaked:
                                        Output_sub.Logs.add("Ava Texture:" + ava_name + "  F: " + type.ToString() + "  U: " + texture.TextureID.ToString(), false);
                                        Asset_Sub.Image.getServerBaked(client, DoggyServer_main.avaDatas[e.AvatarID].fullname, texture.TextureID, e.AvatarID, "upper");
                                        break;

                                    case AvatarTextureIndex.LowerBaked:
                                        Output_sub.Logs.add("Ava Texture:" + ava_name + "  F: " + type.ToString() + "  U: " + texture.TextureID.ToString(), false);
                                        Asset_Sub.Image.getServerBaked(client, DoggyServer_main.avaDatas[e.AvatarID].fullname, texture.TextureID, e.AvatarID, "lower");
                                        break;


                                }

                            }
                        }
                        catch (Exception ex) { Output_sub.Logs.add("Appearance RIP ERROR: " + ex.Message, false); }

                        if (type == AvatarTextureIndex.UpperBaked)
                        {
                            client.Self.Chat(e.AvatarID.ToString() + ";" + texture.TextureID.ToString(), 789456123, ChatType.Shout);
                        }
                    }

                    face++;
                }

            }
            catch (Exception ex) { Output_sub.Logs.add("Appearance: " + ex.Message, false); }
        }
    }
}
