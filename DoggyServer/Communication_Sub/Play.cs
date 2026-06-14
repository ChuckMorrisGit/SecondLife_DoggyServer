using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Play
    {
        public static void command(GridClient client, string[] commandArray, UUID fromAV, string fromName)
        {
            try
            {
                string fullParam = string.Empty;
                int i = 2;
                while (i < commandArray.Count())
                {
                    fullParam += commandArray[i] + " ";
                    i++;
                }
                fullParam = fullParam.TrimEnd(' ');

                switch (commandArray[1])
                {
                    case "ani":
                        UUID ani = UUID.Parse(commandArray[2]);
                        Dictionary<UUID, Boolean> on = new Dictionary<UUID, bool>();
                        Dictionary<UUID, Boolean> off = new Dictionary<UUID, bool>();

                        on.Add(ani, true);
                        off.Add(ani, false);

                        Animation_Sub.Set.off(client);
                        client.Self.Animate(on, false);
                        Movement_Sub.Stand.ava(client);
                        System.Threading.Thread.Sleep(20000);

                        client.Self.Animate(off, false);
                        Animation_Sub.Set.on(client);
                        break;
                }
            }
            catch (Exception ex) { client.Self.InstantMessage(fromAV, "More params? -> " + ex.Message); }
        }
    }
}
