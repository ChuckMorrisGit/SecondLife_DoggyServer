using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyServer.Time_Sub
{
    class Forbit
    {
        

        public static Boolean HomeSim()
        {
            

            Boolean isTime = false;
            DateTime datetime = DateTime.Now;

            switch (datetime.DayOfWeek)
            {
                case DayOfWeek.Friday:
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    if ((datetime.Hour >= 20) || (datetime.Hour < 2)) isTime = true;
                    break;
            }

            return (isTime);
        }
    }
}
