using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class MessageBox
    {
        public static void check(GridClient client, InstantMessage im, Simulator sim)
        {
            Output_sub.Logs.add("Message Box: " + im.Message, false); 

        }
    }
}
