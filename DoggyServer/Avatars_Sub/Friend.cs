using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Avatars_Sub
{
    class Friend
    {
        private static GridClient client;

        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Friends.FriendshipOffered += new EventHandler<FriendshipOfferedEventArgs>(Friends_FriendshipOffered);
        }

        public static void close(GridClient client_new)
        {
            client = client_new;
            client.Friends.FriendshipOffered -= new EventHandler<FriendshipOfferedEventArgs>(Friends_FriendshipOffered);
        }

        static void Friends_FriendshipOffered(object sender, FriendshipOfferedEventArgs e)
        {
            
            client.Friends.AcceptFriendship(e.AgentID, e.SessionID);
        }
    }
}
