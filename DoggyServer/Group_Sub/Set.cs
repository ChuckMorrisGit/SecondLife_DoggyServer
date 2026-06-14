using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Group_Sub
{
    class Set
    {
        public static void normal(GridClient client)
        {
            client.Groups.ActivateGroup(Daten.caprica);

            
        }
    }
}
