using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Inventory
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
                    case "delete":
                        if (commandArray[2] == "norm") Inventory_Sub.Delete.norm(client, fromAV, fromName);
                        break;
                }
            }
            catch (Exception ex) { client.Self.InstantMessage(fromAV, "More params? -> " + ex.Message); }
        }
    }
}
