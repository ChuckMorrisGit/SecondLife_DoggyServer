using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.LSL_Sub
{
    class init
    {
        public static void all(GridClient client)
        {
            try
            {
                Requests.Question.init(client);
                
            }
            catch (Exception ex)
            {
                
                Output_sub.Logs.add("LSL_Sub" + ex.Message , false);

            }
        }
    }
}
