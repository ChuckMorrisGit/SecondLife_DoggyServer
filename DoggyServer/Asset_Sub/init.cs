using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Asset_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Image.init(client);
                Item.init(client);
                Terrain.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Asset_Sub: " + ex.Message, false);
            }

        }
    }

    class close
    {
        public static void all(GridClient client)
        {
            Image.close(client);
            Item.close(client);
            Terrain.close(client);
        }
    }
}
