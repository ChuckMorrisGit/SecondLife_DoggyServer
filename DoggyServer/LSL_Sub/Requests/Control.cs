using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse; 

namespace DoggyServer.LSL_Sub.Requests
{
    class Control
    {
        public static void init(GridClient client)
        {
            client.Self.ScriptControlChange += new EventHandler<ScriptControlEventArgs>(Self_ScriptControlChange);
        }

        static void Self_ScriptControlChange(object sender, ScriptControlEventArgs e)
        {
            Output_sub.Logs.add("Script Control: " + e.Controls.ToString(), false);
        }

    }
}
