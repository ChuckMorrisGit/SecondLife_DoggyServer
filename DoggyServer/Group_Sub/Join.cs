using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Group_Sub
{
    class Join
    {
        public static void init(GridClient client)
        {
            client.Groups.GroupJoinedReply += new EventHandler<GroupOperationEventArgs>(Groups_GroupJoinedReply);
        }

        public static void club(GridClient client)
        {
            try
            {
                if (DoggyServer_main.joinGroupID != UUID.Zero)
                {
                    Output_sub.Logs.add("TRY JOIN GROUP: " + Daten.xFactor.ToString(), false);
                    client.Groups.RequestJoinGroup(DoggyServer_main.joinGroupID);
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("ERROR TRY JOIN GROUP: " + ex.Message, false); };
        }

        static void Groups_GroupJoinedReply(object sender, GroupOperationEventArgs e)
        {
            Output_sub.Logs.add("JOINED GROUP: " + e.GroupID.ToString(),false);
            
        }
    }
}
