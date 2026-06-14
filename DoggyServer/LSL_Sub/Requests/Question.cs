using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.LSL_Sub.Requests
{
    class Question
    {
        public static void init(GridClient client)
        {
            client.Self.ScriptQuestion += new EventHandler<ScriptQuestionEventArgs>(Self_ScriptQuestion);
        }

        static void Self_ScriptQuestion(object sender, ScriptQuestionEventArgs e)
        {
            GridClient client = DoggyServer_main.clients[e.Simulator.Client.Self.AgentID];

            Output_sub.Logs.add("Script Question: " + e.Questions.ToString(), false);
            
            switch (e.Questions)
            {
                case ScriptPermission.TriggerAnimation:
                    client.Self.ScriptQuestionReply(e.Simulator, e.ItemID, e.TaskID, ScriptPermission.TriggerAnimation);        
                    break;
            }
        }
    }
}
