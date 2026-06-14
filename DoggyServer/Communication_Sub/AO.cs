using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class AO
    {
        public static void set(GridClient client, string[] args, UUID fromAva, string fromName)
        {

            switch (args[1].ToLower())
            {
                case "on":
                    if (fromAva != UUID.Zero) client.Self.InstantMessage(fromAva, "AO on");
                    Animation_Sub.Set.on(client);
                    break;

                case "off":
                    Animation_Sub.Set.off(client);
                    if (fromAva != UUID.Zero) client.Self.InstantMessage(fromAva, "AO off");
                    break;

                case "reset":
                    Animation_Sub.Set.reset(client);
                    if (fromAva != UUID.Zero) client.Self.InstantMessage(fromAva, "AO reset");
                    break;

                
            }
        }

    }
}
