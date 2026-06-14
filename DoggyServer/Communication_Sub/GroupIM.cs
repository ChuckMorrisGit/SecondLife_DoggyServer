using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Communication_Sub
{
    class GroupIM
    {
        private static ManualResetEvent WaitForSessionStart = new ManualResetEvent(false);

        public static string say(GridClient client, UUID ToGroupID, string message)
        {
            string error = "";
            try
            {
                client.Self.GroupChatJoined += new EventHandler<GroupChatJoinedEventArgs>(Self_GroupChatJoined);

                if (!client.Self.GroupChatSessions.ContainsKey(ToGroupID))
                {
                    WaitForSessionStart.Reset();
                    client.Self.RequestJoinGroupChat(ToGroupID);
                }
                else
                {
                    WaitForSessionStart.Set();
                }

                if (WaitForSessionStart.WaitOne(20000, false))
                {
                    client.Self.InstantMessageGroup(ToGroupID, message);
                }
                else
                {
                    Output_sub.Logs.add("Timeout waiting for group session start", false);
                    error = "Timeout waiting for group session start";
                }

                client.Self.GroupChatJoined -= Self_GroupChatJoined;
            }
            catch (Exception ex) 
            { 
                Output_sub.Logs.add("JOIN GROUP IM ERROR: " + ex.Message, false);
                error = ex.Message;
            }
            return (error);
        }


        static void Self_GroupChatJoined(object sender, GroupChatJoinedEventArgs e)
        {
            if (e.Success)
            {
                Output_sub.Logs.add("Joined Group: " + e.SessionName, false);
                WaitForSessionStart.Set();
            }
            else
            {
                Output_sub.Logs.add("Join Group Chat failed :(",false);
            }
        }
    }
}
