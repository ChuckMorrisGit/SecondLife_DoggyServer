using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Friends_Sub
{
    class Online
    {
        private static GridClient client;
        public static List<string> friendsOnline = new List<string>();
        public static Boolean notification = true;

        public static void init(GridClient client_new)
        {
            client = client_new;

            client.Friends.FriendOffline += new EventHandler<FriendInfoEventArgs>(Friends_FriendOffline);
            client.Friends.FriendOnline += new EventHandler<FriendInfoEventArgs>(Friends_FriendOnline);
        }

        public static void requestAll(GridClient client)
        {
            client.Friends.FriendList.ForEach(delegate(FriendInfo info)
            {
                Output_sub.Logs.add("CHECK ONLINE FOR: " + info.Name, false);
                checkOnline(client, info.UUID);

                if ((Master.firstLevelMaster.Contains(info.UUID)) && (DoggyServer_main.PrimRechterAnMaster))
                {
                    if ((!info.CanModifyMyObjects) || (!info.CanSeeMeOnline) || (info.CanSeeMeOnMap))
                        client.Friends.GrantRights(info.UUID, FriendRights.CanSeeOnMap | FriendRights.CanSeeOnline | FriendRights.CanModifyObjects);

                }

                if (Database_Sub.ReadAccounts.accounts.Keys.Contains(info.Name.ToLower()))
                {
                    if (Database_Sub.ReadAccounts.accounts[info.Name.ToLower()].uuid == info.UUID)
                    {
                        if ((!info.CanModifyMyObjects) || (!info.CanSeeMeOnline) || (info.CanSeeMeOnMap))
                            client.Friends.GrantRights(info.UUID, FriendRights.CanSeeOnMap | FriendRights.CanSeeOnline | FriendRights.CanModifyObjects);
                    }
                }

                // Delete Evil Friends
                if (DoggyServer_main.avaDatas[info.UUID].masterLevel == 666)
                {
                    client.Friends.TerminateFriendship(info.UUID);
                    Output_sub.Logs.add("TERMINATE FRIENDSHIP: " + DoggyServer_main.avaDatas[info.UUID].fullname, false);
                }
            }
            );
        }

        public static void checkOnline(GridClient client, UUID avaKey)
        {
            client.Friends.RequestOnlineNotification(avaKey);
        }

        static void Friends_FriendOnline(object sender, FriendInfoEventArgs e)
        {
            if (notification) Output_sub.Logs.add(e.Friend.Name + " ist Online", false);
            if (!friendsOnline.Contains(e.Friend.Name)) friendsOnline.Add(e.Friend.Name);

            if (Master.firstLevelMaster.Contains(e.Friend.UUID))
                if (!DoggyServer_main.masters_online.Contains(e.Friend.UUID)) DoggyServer_main.masters_online.Add(e.Friend.UUID);

            if (Group_Sub.GroupMembers.infoAvas.Contains(e.Friend.UUID)) Group_Sub.GroupMembers.sl_inside(client);

            if ((Master.firstLevelMaster.Contains(e.Friend.UUID)) && (DoggyServer_main.agentData.type == AvaData.avaType.doggy))
                Communication_Sub.MasterChat.reply(client, "I have " + client.Self.Balance.ToString() + " L$");
        }

        static void Friends_FriendOffline(object sender, FriendInfoEventArgs e)
        {
            if (notification) Output_sub.Logs.add(e.Friend.Name + " ist Offline", false);
            if (friendsOnline.Contains(e.Friend.Name)) friendsOnline.Remove(e.Friend.Name);
            if (e.Friend.Name == Master.currentMaster)
            {
                Teleport_Sub.Home.go(client);
            }

            if (Master.firstLevelMaster.Contains(e.Friend.UUID))
                if (DoggyServer_main.masters_online.Contains(e.Friend.UUID)) DoggyServer_main.masters_online.Remove(e.Friend.UUID);

        }

        public static Boolean check(GridClient client, UUID key)
        {
            Boolean isOnline = false;

            client.Friends.FriendList.ForEach(delegate(FriendInfo info)
                   {
                       if ((info.UUID == key) && (info.IsOnline)) isOnline = true;
                   }
            );

            return (isOnline);
        }
    }
}
