using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
namespace DoggyServer.Simulator_Sub
{
    class Terrain
    {
        public static void init(GridClient client)
        {
            try { 
            client.Terrain.LandPatchReceived += new EventHandler<LandPatchReceivedEventArgs>(Terrain_LandPatchReceived);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Simulator_Sub->Terrain: " + ex.Message, false);
            }
        }

        static void Terrain_LandPatchReceived(object sender, LandPatchReceivedEventArgs e)
        {
            
        }

        public static void getRAW(GridClient client)
        {
            


        }
    }
}
