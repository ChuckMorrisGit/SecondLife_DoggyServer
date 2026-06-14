using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub.Target
{
    class Parse
    {
        public static void target(GridClient client, string art, string target, UUID fromAva)
        {
            int i = 0;
            Boolean found = false;

            foreach (string wayPointName in Data.waypoints.Keys)
            {
                string[] nameArray = wayPointName.Split(':');

                if ((nameArray[0] == art) && (nameArray[1].Contains(target)) && (!found))
                {
                    found=true;

                    client.Self.InstantMessage(fromAva, "Im going to:" + nameArray[0] + " -> " + nameArray[1]);

                    Go.to(client, wayPointName,fromAva);
                }
            }

            if (!found) client.Self.InstantMessage(fromAva, "No Target found with this name");
        }
    }
}
