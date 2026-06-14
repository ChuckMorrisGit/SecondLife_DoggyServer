using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Simulator_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                ParcelUpdate.init(client);
                SimUpdate.init(client);
                Media_Sub.MusicURL.init(client);
                Sim.init(client);
                ParcelObjectOwner.init(client);
                Terrain.init(client);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Simulator_Sub: " + ex.Message, false);
            }

        }
    }
}
