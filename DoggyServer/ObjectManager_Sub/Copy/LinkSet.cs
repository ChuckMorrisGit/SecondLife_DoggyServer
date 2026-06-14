using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.ObjectManager_Sub.Copy
{
    class LinkSet
    {
            public Primitive RootPrim;
            public List<Primitive> Children = new List<Primitive>();

            public LinkSet()
            {
                RootPrim = new Primitive();
            }

            public LinkSet(Primitive rootPrim)
            {
                RootPrim = rootPrim;
            }
    }
}
