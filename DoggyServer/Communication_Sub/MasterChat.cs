using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class MasterChat
    {


        public static void reply(GridClient client, string text)
        {


            foreach (UUID uuid in Master.firstLevelMaster)
            {
                    if (Friends_Sub.Online.check(client, uuid)) client.Self.InstantMessage(uuid, text);
            }

        }
    }
}
