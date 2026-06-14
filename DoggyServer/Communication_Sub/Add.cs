using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Add
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
                    case "ava":
                        Avatars_Sub.NewAvatar.addUnknown(client, new UUID (commandArray[2]));
                        break;
                }
            }
            catch (Exception ex) { client.Self.InstantMessage(fromAV, "More params? -> " + ex.Message); }
        }

    }
}
