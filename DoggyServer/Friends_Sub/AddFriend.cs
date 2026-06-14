using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Friends_Sub
{
    class AddFriend
    {
        public static void master(GridClient client)
        {
            if (Friends_Sub.init.addMaster)
            {
                foreach (UUID uuid in Master.firstLevelMaster)
                {

                    if (!client.Friends.FriendList.ContainsKey(uuid))
                    {
                        Communication_Sub.MasterChat.reply(client, "Add Master as Friend: " + DoggyServer_main.avaDatas[uuid].fullname);
                        client.Friends.OfferFriendship(uuid, "Hello. You know Walter?");
                    }
                }
            }
        }

        public static void OtherBots(GridClient client)
        {
            if (Friends_Sub.init.addBots)
            {
                foreach (string name in Database_Sub.ReadAccounts.accounts.Keys)
                {
                    //Communication_Sub.MasterChat.reply(client, "Try: " + name);
                    UUID uuid = Database_Sub.ReadAccounts.accounts[name].uuid;
                    if ((!client.Friends.FriendList.ContainsKey(uuid)) && (uuid != UUID.Zero) && (uuid != client.Self.AgentID))
                    {
                        client.Friends.OfferFriendship(uuid, "Hello. You know Walter?");
                        Communication_Sub.MasterChat.reply(client, "Add Kerlchen as Friend: " + name);
                    }
                }
            }
        }
    }
}
