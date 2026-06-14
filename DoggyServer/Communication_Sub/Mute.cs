using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Mute
    {
        public static void to(GridClient client, string[] commandArray, UUID fromAV, string fromName)
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
                    case "add":
                        UUID avaKey = UUID.Parse(commandArray[2]);
                        Mute_Sub.AddKey.add(avaKey);
                        client.Self.InstantMessage(fromAV, "Add " + DoggyServer_main.avaDatas[avaKey].fullname +" to Mutelist");
                        break;

                    case "show":
                        foreach (string avaName in DoggyServer_main.muteListByName)
                            client.Self.InstantMessage(fromAV, avaName);
                        break;

                }
            }
            catch (Exception ex) { client.Self.InstantMessage(fromAV, "More params? -> " + ex.Message); }
        }
    }
}
