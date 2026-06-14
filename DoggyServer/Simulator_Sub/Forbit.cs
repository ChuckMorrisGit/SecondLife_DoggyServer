using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Simulator_Sub
{
    class Forbit
    {
        private static DateTime forbitTime = DateTime.Now;
        private static Boolean forbit = false;
        public static Boolean allowAllways = true;

        public static void check(GridClient client)
        {
            if (!allowAllways)
            {
                if (ParcelUpdate.simName == "Grand Hustle")
                {
                    //if (Time_Sub.Forbit.HomeSim()) Teleport_Sub.Home.go(client);

                    if ((ParcelUpdate.parcelProperties.Simulator.ObjectsAvatars.Count > 6) || (Time_Sub.Forbit.HomeSim()))
                    {
                        forbit = true;
                        Teleport_Sub.Home.tpNormalHome = false;
                        forbitTime = DateTime.Now;
                        Teleport_Sub.Home.go(client);
                    }
                    else
                    {
                        forbit = false;
                        Teleport_Sub.Home.tpNormalHome = true;
                    }
                }

                if (forbit)
                {
                    TimeSpan timeSpan = DateTime.Now - forbitTime;
                    if ((timeSpan.TotalHours >= 1) || (!Time_Sub.Forbit.HomeSim()))
                    {
                        forbit = false;
                        Teleport_Sub.Home.tpNormalHome = true;
                        Teleport_Sub.Home.go(client);
                    }
                }
            }
            else
            {
                forbit = false;
                Teleport_Sub.Home.tpNormalHome = true;
            }
        }
    }
}
