using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyServer.Database_Sub
{
    class init
    {
        public static void all()
        {
            try
            {
                Output_sub.Logs.add("Read Masters", false);
                ReadMasters.all();

                Output_sub.Logs.add("Read Sims", false);
                ReadSims.all();

                ReadAccounts.init();
                //ReadOld.all();
            }
            catch (Exception ex) { 
                Output_sub.Logs.add(ex.ToString(), false); 
            }
        }
    }
}
