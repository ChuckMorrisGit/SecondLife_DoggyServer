using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Avatars_Sub
{
    class OnlineCheck
    {
        private static GridClient client;
        public static void init(GridClient client_new)
        {
            client = client_new;

        }

        private static int check_counter = 0;
        public static void CheckNext()
        {
            client.Avatars.RequestAvatarProperties(DoggyServer_main.avasOnlineList[check_counter]);

            check_counter++;
            if (check_counter >= DoggyServer_main.avasOnlineList.Count) check_counter = 0;
        }
    }
}
