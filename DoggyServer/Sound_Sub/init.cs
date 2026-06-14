using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Sound_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Play.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Sound_Sub: " + ex.Message, false);
            }

        }
    }
}
