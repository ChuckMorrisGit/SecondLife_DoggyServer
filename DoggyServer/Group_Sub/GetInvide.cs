using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Group_Sub
{
    class GetInvide
    {
        public static void init(GridClient client)
        {
            client.Groups.GroupInvitation += new EventHandler<GroupInvitationEventArgs>(Groups_GroupInvitation);
        }

        static void Groups_GroupInvitation(object sender, GroupInvitationEventArgs e)
        {
            UUID fromAV = e.AgentID; 
            
            
            Output_sub.Logs.add("GROUP INVIDE from " +  e.FromName + "  " + fromAV.ToString(), false);
            if (e.FromName == "walter.ginsburg")
            {
                e.Accept = true;

                Group_Sub.Set.normal(e.Simulator.Client);
            }
        }
    }
}
