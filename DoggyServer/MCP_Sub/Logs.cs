using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.MCP_Sub
{
    class Logs
    {
        public static void add(GridClient client, string line)
        {
            
            if (init.use_remoting)
            {
                //Console.WriteLine("\n TRY REMOTE\n");
                try
                {
                    if (client == null) init.service.log_add(UUID.Zero, line);
                    else init.service.log_add(client.Self.AgentID, line);

                }
                catch (Exception ex) 
                {
                    Console.WriteLine("\n" + ex.Message + "\n");
                    //Environment.Exit(0);
                    init.RegisterService();
                }
            }
        }

    }
}
