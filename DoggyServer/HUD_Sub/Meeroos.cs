using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.HUD_Sub
{
    class Meeroos
    {
        public static void pet(GridClient client, UUID fromAva)
        {
            uint rootPrim = Scan.hudRootPrim(client, "WW of Meeroos HUD V1.0", fromAva);

            if (rootPrim != 0)
            {
                client.Self.Touch(rootPrim);
                
                
            }
        }
    }
}
