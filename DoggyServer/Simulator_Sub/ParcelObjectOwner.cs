using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Simulator_Sub
{
    class ParcelObjectOwner
    {
        public static List<ParcelManager.ParcelPrimOwners> primOwners = new List<ParcelManager.ParcelPrimOwners>();
        public static void init(GridClient client)
        {
            try
            {
                client.Parcels.ParcelObjectOwnersReply += new EventHandler<ParcelObjectOwnersReplyEventArgs>(Parcels_ParcelObjectOwnersReply);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Simulator_Sub->ParcelObjectOwner: " + ex.Message, false);
            }
        }

        public static void refreshOwners(GridClient client)
        {
            //if (ParcelUpdate.parcelID!=0) client.Parcels.RequestObjectOwners(client.Network.CurrentSim, ParcelUpdate.parcelID);

        }

        static void Parcels_ParcelObjectOwnersReply(object sender, ParcelObjectOwnersReplyEventArgs e)
        {
            primOwners = e.PrimOwners;
        }

        public static List<string> getOwners()
        {
            List<string> owners = new List<string>();

            foreach (ParcelManager.ParcelPrimOwners primOwner in primOwners)
            {
                if (DoggyServer_main.avaDatas.ContainsKey(primOwner.OwnerID)) owners.Add(DoggyServer_main.avaDatas[primOwner.OwnerID].fullname);
                else owners.Add(primOwner.OwnerID.ToString());

            }

            return (owners);
        }

    }
}
