using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class init
    {
        public static Boolean fetchInvetory = false;

        public static void all(GridClient client)
        {
            Offered.init(client);
            if (fetchInvetory) Fetch.all(client, UUID.Zero, false);

            
        }
    }
}
