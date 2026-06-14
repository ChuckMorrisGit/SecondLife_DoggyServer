using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Simulator_Sub
{
    class Parcel
    {
        public static Boolean CheckIfOwn(GridClient client)
        {
            Boolean isOwn = false;


            //if ((Simulator_Sub.ParcelUpdate.parcelName.Contains("X-Factor Club Area"))
            //        && (Simulator_Sub.ParcelUpdate.simName == "Grand Hustle")) isOwn = true;
            

            if (Simulator_Sub.ParcelUpdate.simName == "Pinedown") isOwn = true;

            return (isOwn);
        }
    }
}
