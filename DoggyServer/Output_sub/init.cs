using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Output_sub
{
    class init
    {
        public static void all(GridClient client)
        {
            LogFile.init(client);
            Logs.init(client);
            MainScreen.init(client);
        }
    }
}
