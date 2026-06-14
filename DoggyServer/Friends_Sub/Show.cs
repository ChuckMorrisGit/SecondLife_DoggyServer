using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Friends_Sub
{
    class Show
    {
        public static void all(GridClient client, UUID fromAva)
        {
            if (client.Friends.FriendList.Count > 0)
            {
                client.Friends.FriendList.ForEach(delegate(FriendInfo friend)
                {
                    client.Self.InstantMessage(fromAva, friend.Name + ": " + friend.IsOnline.ToString());
                });
            }
        }
    }
}
