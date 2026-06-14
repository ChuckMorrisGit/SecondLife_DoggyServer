using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Appearance_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Manager.init(client);
                Shape.init(client);
                WearAble.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Appearance_Sub: " + ex.Message, false);
            }
        }
    }

    class close
    {
        public static void all(GridClient client)
        {
            Manager.close(client);
            WearAble.close(client);
        }
    }

}
