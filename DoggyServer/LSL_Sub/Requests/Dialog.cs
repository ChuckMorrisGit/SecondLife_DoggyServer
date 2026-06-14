using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse; 

namespace DoggyServer.LSL_Sub.Requests
{
    class Dialog
    {
        public static void init(GridClient client)
        {
            client.Self.ScriptDialog += new EventHandler<ScriptDialogEventArgs>(Self_ScriptDialog);
        }

        static void Self_ScriptDialog(object sender, ScriptDialogEventArgs e)
        {
            Output_sub.Logs.add("Script Dialog: " + e.Message, false);
        }

    }
}
