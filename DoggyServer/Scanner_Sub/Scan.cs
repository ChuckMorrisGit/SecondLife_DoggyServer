using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Scanner_Sub
{
    class Scan
    {
        private static Random rand = new Random();

        public static void sims(GridClient client)
        {
            int whichSim = rand.Next(0, DoggyServer_main.simDatas.Count);
            int xPos = rand.Next(0, 255);
            int yPos = rand.Next(0, 255);
            int zPos = rand.Next(80, 255);

            client.Self.Fly(true);

            List<string> sims = DoggyServer_main.simDatas.Keys.ToList();
            client.Self.Teleport(sims[whichSim] ,new Vector3( xPos,yPos,zPos));

            for (int i = 0; i < 6; i++)
            {
                Avatars_Sub.Properties.update(client);
                System.Threading.Thread.Sleep(10000);
                xPos = rand.Next(0, 255);
                yPos = rand.Next(0, 255);

                Movement_Sub.Move.to(client, new Vector3(xPos, yPos, zPos));
            }
        }
    }
}
