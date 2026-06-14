using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyServer
{
    class PublicSims
    {
        public static List<string> SimNames = new List<string>();
        private static Random rand = new Random();

        public static void init()
        {
            SimNames.Add("Ross");
            SimNames.Add("Orientation Island Public");
            //SimNames.Add("Mann");
            SimNames.Add("Bear Dream Lodge");
            SimNames.Add("Braunworth");
            SimNames.Add("Calleta Hobo Railroad");
            //            SimNames.Add("Governor Lindens Mansion");
            SimNames.Add("Hyles Swamp");
            SimNames.Add("Temple of Iris");
            SimNames.Add("Isabel");
            SimNames.Add("Mahulu");
            SimNames.Add("Mauve");
            SimNames.Add("Miramare");
            SimNames.Add("Periwinkle Infohub");
            SimNames.Add("Ross");
            SimNames.Add("Violet");
            SimNames.Add("Warmouth");
            SimNames.Add("Wengen");
            SimNames.Add("Miramare Bay");
            SimNames.Add("Straits of Shermerville");
            SimNames.Add("Barcola Sound");
            SimNames.Add("Cape Haven");
            SimNames.Add("Ahern");
            SimNames.Add("Hanja");
            SimNames.Add("Hangeul");
            SimNames.Add("Plum");
            SimNames.Add("Violet");
            SimNames.Add("Waterhead");
        }

        public static string getRandom()
        {
            string name = SimNames[rand.Next(0, SimNames.Count)];
            return (name);
        }
    }
}
