using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Map_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                GridRegion.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Map_Sub: " + ex.Message, false);
            }
        }
    }
}
