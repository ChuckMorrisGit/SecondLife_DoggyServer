using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Animation_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                AO.init(client);
                Rip.init(client);
                RequestSit.init(client);
                Data.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Animation_Sub: " + ex.Message, false);
            }
        }
    }

    class close
    {
        public static void all(GridClient client)
        {
            Rip.close(client);
        }
    }

}
