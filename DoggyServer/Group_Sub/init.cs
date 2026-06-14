using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Group_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                OwnGroups.init(client);
                GroupMembers.init(client);
                GetInvide.init(client);
                Cache.reload(client);

                Join.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Group_Sub: " + ex.Message, false);
            }
        }
    }
}
