using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Variables_Sub
{
    class init
    {
        public static void all()
        {
            addGroup(Group_Sub.Daten.caprica);
            addGroup(Group_Sub.Daten.xFactor);
        }

        private static void addGroup(UUID group)
        {
            if (!Group_Sub.GroupMembers.membersInGroup.ContainsKey(group)) Group_Sub.GroupMembers.membersInGroup.Add(group, new List<UUID>());
        }
    }
}
