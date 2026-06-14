using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMetaverse;

namespace DoggyServer.ObjectManager_Sub
{
    class GridSearch
    {
        public static UUID[] ownerIDs = { new UUID("11111111-2222-3333-4444-555555555555"), new UUID("11111111-2222-3333-4444-555555555555") };
        public static void ObjectByOwnerID(GridClient client, Primitive ob)
        {
            Output_sub.Logs.add("TRY FIND PRIM PER OwnerID", false);

            try
            {
                if (ownerIDs.Contains(ob.OwnerID))
                {
                    Output_sub.Logs.add("FOUND: " + ob.OwnerID.ToString(), false);

                    Communication_Sub.MasterChat.reply(client, "FOUND: " + ob.OwnerID.ToString());


                    Communication_Sub.Email.send("FOUND PRIMITIVE", "FROM " + ob.OwnerID + "\nON: " + Simulator_Sub.ParcelUpdate.simName);
                }
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR GridSearch: " + ex.Message, false);
            }
        }
    }
}
