using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Simulator_Sub
{
    class ParcelUpdate
    {
        private static GridClient client;
        public static string parcelName = string.Empty;
        public static int parcelID = 0;
        public static string simName = string.Empty;
        public static string musikURL = string.Empty;
        public static ParcelPropertiesEventArgs parcelProperties = null;
        public static ParcelFlags flags = new ParcelFlags();
        public static string colo = string.Empty;

        public static void init(GridClient client_new)
        {
            try
            {
                client = client_new;

                client.Parcels.ParcelProperties += new EventHandler<ParcelPropertiesEventArgs>(Parcels_ParcelProperties);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Simulator_Sub->ParcelUpdate: " + ex.Message, false);
            }
        }


        static void Parcels_ParcelProperties(object sender, ParcelPropertiesEventArgs e)
        {
            simName = e.Simulator.Name;
            parcelName = e.Parcel.Name;
            musikURL = e.Parcel.MusicURL;
            flags = e.Parcel.Flags;
            parcelProperties = e;
            parcelID = e.Parcel.LocalID;
            colo = e.Simulator.ColoLocation;
            
            //Output_sub.MainScreen.Output(client);

            Media_Sub.MusicURL.check(e.Simulator.Name, e.Parcel.Name, e.Parcel.MusicURL);
            ParcelObjectOwner.refreshOwners(client);
        }
    }
}
