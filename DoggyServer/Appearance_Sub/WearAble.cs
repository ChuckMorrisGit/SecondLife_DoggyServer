using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Appearance_Sub
{
    class WearAble
    {
        public static void init(GridClient client)
        {
            client.Appearance.AgentWearablesReply += new EventHandler<AgentWearablesReplyEventArgs>(Appearance_AgentWearablesReply);
        }

        public static void close(GridClient client)
        {
            client.Appearance.AgentWearablesReply -= new EventHandler<AgentWearablesReplyEventArgs>(Appearance_AgentWearablesReply);
        }

        static void Appearance_AgentWearablesReply(object sender, AgentWearablesReplyEventArgs e)
        {
            
        }
    }
}
