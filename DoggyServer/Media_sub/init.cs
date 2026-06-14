using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Media_sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Lyrics.init(client);
                Metadata.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Media_Sub: " + ex.Message, false);
            }
        }
    }
}
