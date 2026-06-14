using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Packets;
//using OpenMetaverse.Utilities;
using System.Threading;

namespace DoggyServer.Group_Sub
{
    class Cache
    {
        public static Dictionary<UUID, Group> groupsCache = new Dictionary<UUID, Group>();
        private static ManualResetEvent groupsEvent = new ManualResetEvent(false);

        public static void reload(GridClient client)
        {
            groupsEvent.Reset();
            client.Groups.CurrentGroups += new EventHandler<CurrentGroupsEventArgs>(Groups_CurrentGroups);
            client.Groups.RequestCurrentGroups();
            groupsEvent.WaitOne(10000, false);
            client.Groups.CurrentGroups -= new EventHandler<CurrentGroupsEventArgs>(Groups_CurrentGroups);
            groupsEvent.Reset();
        }

        static void Groups_CurrentGroups(object sender, CurrentGroupsEventArgs e)
        {
            if (null == groupsCache)
                groupsCache = e.Groups;
            else
                lock (groupsCache) { groupsCache = e.Groups; }
            groupsEvent.Set();
        }
    }
}
