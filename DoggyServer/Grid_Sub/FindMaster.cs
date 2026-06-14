using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Grid_Sub
{
    class FindMaster
    {
        private static GridClient client;
        private static Vector3 masterSimPos;
        private static string masterSimName = string.Empty;
        private static ulong regionHandle = 0;

        private static int masterFindTimeout = 60;
        public static DateTime masterLastFound = DateTime.Now;

        public static void init(GridClient client_new)
        {
            client = client_new;
        }

        public static void TimerTask()
        {
            if (ObjectManager_Sub.ObjectUpdate.followOn)
            {
                TimeSpan timeSpan = DateTime.Now - masterLastFound;
                //Output_sub.Logs.add(timeSpan.TotalSeconds.ToString());
                if (timeSpan.TotalSeconds > masterFindTimeout)
                {
                    Output_sub.Logs.add("Mapping my Master", false);
                    MapAndTeleport();
                }
            }
        }

        private static Boolean mapping = false;
        public static void MapAndTeleport()
        {
            if (!mapping)
            {
                mapping = true;
                client.Friends.FriendFoundReply += new EventHandler<FriendFoundReplyEventArgs>(Friends_FriendFoundReply);
                regionHandle = 0;

                if (Master.masters.Keys.Contains(Master.currentMaster))
                {
                    client.Friends.MapFriend(Master.masters[Master.currentMaster].uuid);

                    int timeout = 60;
                    while ((timeout > 0) && (regionHandle == 0))
                    {
                        Output_sub.Logs.add("Scanning for my Master " + Master.currentMaster, false);
                        System.Threading.Thread.Sleep(1000);
                        timeout--;
                    }
                }
                else
                {
                    Output_sub.Logs.add("No key for " + Master.currentMaster, false);
                }
                client.Friends.FriendFoundReply -= new EventHandler<FriendFoundReplyEventArgs>(Friends_FriendFoundReply);
                mapping = false;
            }
        }

        static void Friends_FriendFoundReply(object sender, FriendFoundReplyEventArgs e)
        {
            if (!e.RegionHandle.Equals(0))
            {
                masterLastFound = DateTime.Now;
                regionHandle = e.RegionHandle;

                string locationText = 
                    "Walter @ " + ": " + e.Location.X.ToString() + " " + e.Location.Y.ToString() + " " + e.Location.Z.ToString();
                masterSimPos = e.Location;
                masterSimPos.Z += 2;


                if (client.Network.CurrentSim.Handle != e.RegionHandle)
                {
                    Output_sub.Logs.add(locationText + "On a diffrent Region. Teleporting", false);
                    client.Self.Teleport(e.RegionHandle, masterSimPos);
                }
                else
                {
                    Output_sub.Logs.add(locationText + "On the same Region", false);
                    Movement_Sub.Follow.master(client, new Vector3(masterSimPos.X, masterSimPos.Y, client.Self.SimPosition.Z), false);
                }
            }
            else
            {
                Output_sub.Logs.add(Master.currentMaster + " might be offline", false);
            }
        }
    }
}
