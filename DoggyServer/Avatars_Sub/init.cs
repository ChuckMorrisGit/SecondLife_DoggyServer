using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Avatars_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Friend.init(client);
                OnSim.init(client);
                Avatar.init(client);
                Appearance.init(client);
                Properties.init(client);
                Search.init(client);
                OnlineCheck.init(client);
                View.init(client);
                NewAvatar.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Avatars_Sub: " + ex.Message, false);
            }
        }
    }

    class close
    {
        public static void all(GridClient client)
        {
            Friend.close(client);
            OnSim.close(client);
            Avatar.close(client);
            Appearance.close(client);
            Properties.close(client);
            Search.close(client);
            //OnlineCheck.init(client);
            View.close(client);
            NewAvatar.close(client);
        }
    }

}
