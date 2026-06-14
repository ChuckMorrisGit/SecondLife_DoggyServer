using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Teleport_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Teleport.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Teleport_Sub: " + ex.Message, false);
            }
        }
    }
}
