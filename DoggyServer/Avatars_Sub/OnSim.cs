using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
namespace DoggyServer.Avatars_Sub
{
    class OnSim
    {
        private static GridClient client;

        public static void init(GridClient client_new)
        {
            client = client_new;

            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
        }

        public static void close(GridClient client_new)
        {
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
        }

        static void Avatars_UUIDNameReply(object sender, UUIDNameReplyEventArgs e)
        {
            Dictionary<UUID,string> avas = e.Names;

            foreach (UUID avaID in avas.Keys)
            {
                //Output_sub.Logs.add("On Sim: " + avas[avaID], false);
            }
        }
    }
}
