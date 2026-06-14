using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Send
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
                    case "notice":
                        Group_Sub.Notice.sendUUID(client, new UUID("88463c14-0398-123b-8cfc-6a5c31bb3ed6"), "Test Notice", "Geht's oder geht's nicht");
                        break;
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("Group Notice: " + ex.Message, false); }
        }
    }
}
