using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenMetaverse;

namespace DoggyServer.Animation_Sub
{
    class Rip
    {
        public static string subDir_save = "../Animations/";
        public static Dictionary<UUID, List<Animation>> anis = new Dictionary<UUID, List<Animation>>();
        private static GridClient client;

        public static void init(GridClient client_new)
        {
            client = client_new;

            client.Avatars.AvatarAnimation += new EventHandler<AvatarAnimationEventArgs>(Avatars_AvatarAnimation);
        }

        public static void close(GridClient client)
        {
            client.Avatars.AvatarAnimation -= new EventHandler<AvatarAnimationEventArgs>(Avatars_AvatarAnimation);
        }

        static void Avatars_AvatarAnimation(object sender, AvatarAnimationEventArgs e)
        {
            List<Animation> aniList = e.Animations;
            UUID avaKey = e.AvatarID;
            
            if (anis.ContainsKey(avaKey)) anis[avaKey] = aniList;
            else anis.Add(avaKey,aniList);

            string ava_name = "Unknown";
            if (DoggyServer_main.avaDatas.ContainsKey(avaKey)) ava_name = DoggyServer_main.avaDatas[avaKey].fullname.Replace(' ', '_');

            foreach (Animation ani in aniList)
            {
                Asset_Sub.Ani.download(client, ani.AnimationID, "Animation_" + ava_name + "_");
                
            }
        }

        public static void save(GridClient client)
        {
            if (!Directory.Exists(subDir_save)) Directory.CreateDirectory(subDir_save);

            foreach (UUID avaKey in anis.Keys)
            {
                DateTime dateTime = DateTime.Now;
                string timestamp = dateTime.Year.ToString() + "_" + dateTime.Month.ToString() + "_" + dateTime.Day.ToString()
                    + dateTime.Hour.ToString() + dateTime.Minute.ToString();

                string fullname = DoggyServer_main.avaDatas[avaKey].vorname + "_" + DoggyServer_main.avaDatas[avaKey].nachname;

                string filename = subDir_save + fullname + "_" + timestamp + ".ani";

                StreamWriter sw = new StreamWriter(filename);

                List<Animation> aniList = anis[avaKey];
                string fileContent = string.Empty;
                foreach (Animation ani in aniList)
                {
                    sw.WriteLine(ani.AnimationID.ToString() + ":" + ani.AnimationSequence.ToString() + ":" + ani.AnimationSourceObjectID.ToString());
                }
                sw.Close();

                Output_sub.Logs.add("Save Animation: " + filename, false);
            }
        }
    }
}
