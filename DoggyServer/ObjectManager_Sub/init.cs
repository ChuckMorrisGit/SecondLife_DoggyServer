using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.ObjectManager_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                ObjectUpdate.init(client);
                Copy.Rez.init(client);
            }catch (Exception ex) { Output_sub.Logs.add("ERROR ObjectManager_Sub: " + ex.Message, false); }
        }
    }

    class close
    {
        public static void all(GridClient client)
        {
            ObjectUpdate.close(client);
            Copy.Rez.close(client);
        }
    }
}
