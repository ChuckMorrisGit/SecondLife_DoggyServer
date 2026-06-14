using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Group_Sub
{
    class OwnGroups
    {
        public static Dictionary<UUID, Group> groups = new Dictionary<UUID, Group>();
        private static GridClient client;

        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Groups.CurrentGroups += new EventHandler<CurrentGroupsEventArgs>(Groups_CurrentGroups);
            client.Groups.GroupJoinedReply += new EventHandler<GroupOperationEventArgs>(Groups_GroupJoinedReply);
            RequestCurrentGroups(client);
        }

        public static void RequestCurrentGroups(GridClient client)
        {
            client.Groups.RequestCurrentGroups();
        }

        static void Groups_GroupJoinedReply(object sender, GroupOperationEventArgs e)
        {
            try
            {
                Output_sub.Logs.add("Join Group: " + groups[e.GroupID].Name + "  " + e.Success.ToString(), false);
            }
            catch (Exception ex) { };
        }

        static void Groups_CurrentGroups(object sender, CurrentGroupsEventArgs e)
        {
            groups = e.Groups;

            if (!e.Groups.ContainsKey(Daten.caprica))
            {
                client.Groups.RequestJoinGroup(Daten.caprica);
            }

            foreach (UUID group_id in groups.Keys)
            {
                MCP_Sub.Messages.group(client, groups[group_id]);
            }
        }
    }
}
