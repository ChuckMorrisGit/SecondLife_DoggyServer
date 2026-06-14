using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Help
    {
        public static void output(GridClient client, UUID fromAV)
        {
            if (Master.firstLevelMaster.Contains(fromAV))
            {
                client.Self.InstantMessage(fromAV, "balance             get L$ amount");
                client.Self.InstantMessage(fromAV, "give >amount<       give >amount< L$");
                client.Self.InstantMessage(fromAV, "teleport            send teleport");
            }

            client.Self.InstantMessage(fromAV, "follow >lastmane<   your Doggy follow you. lastname for another Doggy");
            client.Self.InstantMessage(fromAV, "stay >lastmane<     your Doggy stay here. lastname for another Doggy");
            client.Self.InstantMessage(fromAV, "say >text<     Say >text< in Chat");
            client.Self.InstantMessage(fromAV, "shout >text<     Shout >text< in Chat");

        }
    }
}
