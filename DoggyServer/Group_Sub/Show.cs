using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Group_Sub
{
    class Show
    {
        public static void all(GridClient client, UUID fromAva)
        {
            Cache.reload(client);

            foreach (Group group in Cache.groupsCache.Values)
            {
                client.Self.InstantMessage(fromAva, group.Name);
            }
        }
    }
}
