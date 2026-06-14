using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.LoginManager_Sub
{
    class RunParameter
    {
        public static void set(GridClient client)
        {
            if (client.Self.Name == "Hund Avatar")
            {
                Animation_Sub.AO.standTimer = 60;
                Animation_Sub.AO.nothing2DoTimer = 150;

            }
        }
    }
}
