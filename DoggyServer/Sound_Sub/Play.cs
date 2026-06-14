using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Sound_Sub
{
    class Play
    {
        public static void init(GridClient client)
        {
            
        }

        public static void bellen(GridClient client)
        {
            client.Sound.PlaySound(new UUID("7d523ee8-1921-e7cb-8f8a-b948a6c02fde"));
        }
    }
}
