using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Avatars_Sub
{
    class Get
    {
        public static UUID name2key(string fullname)
        {
            UUID key = UUID.Zero;

            if (DoggyServer_main.avaNames.ContainsKey(fullname)) key = DoggyServer_main.avaNames[fullname];

            return (key);
        }
    }
}
