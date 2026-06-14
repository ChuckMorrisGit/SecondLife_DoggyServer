using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Spanker
    {
        public static void bite(GridClient client, Vector3 pos)
        {
            Movement_Sub.Move.to(client, pos);

            Sound_Sub.Play.bellen(client);
        }
    }
}
