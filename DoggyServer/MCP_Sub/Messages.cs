using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.MCP_Sub
{
    class Messages
    {
        public static void chat(GridClient client, UUID session_id, string line)
        {
            if (init.use_remoting)
            {
                try
                {
                    if (!init.service.add_chat(client.Self.AgentID, session_id, line))
                    {
                        init.init_mcp(client);
                    }

                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("MCP CHAT ERROR: " + ex.Message, false);
                    init.RegisterService();
                }
            }

        }
        public static void im(GridClient client, UUID session_id, string line)
        {
            if (init.use_remoting)
            {
                try
                {
                    if (!init.service.add_im(client.Self.AgentID, session_id, line))
                    {
                        init.init_mcp(client);
                    }

                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("MCP IM ERROR: " + ex.Message, false);
                    init.RegisterService();
                }
            }

        }

        public static void g_im(GridClient client, UUID session_id, string line)
        {
            if (init.use_remoting)
            {
                try
                {
                    if (!init.service.add_g_im(client.Self.AgentID, session_id, line))
                    {
                        init.init_mcp(client);
                    }

                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("MCP GROUP IM ERROR: " + ex.Message, false);
                    init.RegisterService();
                }
            }

        }

        public static void group(GridClient client, Group group)
        {
            if (init.use_remoting)
            {
                DoggyMCP.Comunication_Sub.Group_data group_data = new DoggyMCP.Comunication_Sub.Group_data();
                group_data.group_id = group.ID;
                group_data.group_name = group.Name;

                try
                {
                    if (!init.service.add_group(client.Self.AgentID, group_data))
                    {
                        init.init_mcp(client);
                    }

                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("MCP GROUP ERROR: " + ex.Message, false);
                    init.RegisterService();
                }
            }

        }
    }
}
