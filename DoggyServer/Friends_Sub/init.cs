using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Friends_Sub
{
    class init
    {
        public static Boolean addMaster = false;
        public static Boolean addBots = false;

        public static void all(GridClient client)
        {
            try
            {
                Online.init(client);
                GetInvide.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Friends_Sub: " + ex.Message, false);
            }
        }
    }
}
