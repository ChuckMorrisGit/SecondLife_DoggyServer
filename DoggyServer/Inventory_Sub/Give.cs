using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Inventory_Sub
{
    class Give
    {
        public static void itemTo(GridClient client, UUID itemKey, string avaFullname)
        {
            UUID avaKey = Avatars_Sub.Get.name2key(avaFullname);

            //client.Self.InstantMessage(avaKey, "Get an Item");
        }
    }
}
