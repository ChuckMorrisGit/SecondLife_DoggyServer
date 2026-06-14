using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.MCP_Sub
{
    class Command
    {
        public static void get(GridClient client)
        {
            if (init.use_remoting)
            {
                try
                {
                    DoggyMCP.MCP_Data.GetCommand command = init.service.get_command(client.Self.AgentID);

                    switch (command)
                    {
                        case DoggyMCP.MCP_Data.GetCommand.say_chat:
                            List<string> chat_texte = init.service.get_say_chat(client.Self.AgentID);
                            foreach (string line in chat_texte)
                            {
                                client.Self.Chat(line, 0, ChatType.Normal);
                            }
                            break;

                        case DoggyMCP.MCP_Data.GetCommand.say_im:
                            List<DoggyMCP.Comunication_Sub.Com_data> com_datas = init.service.get_say_im(client.Self.AgentID);
                            foreach (DoggyMCP.Comunication_Sub.Com_data com_data in com_datas)
                            {
                                if (Group_Sub.OwnGroups.groups.Keys.Contains(com_data.session_id))
                                {
                                    Output_sub.Logs.add(Group_Sub.OwnGroups.groups[com_data.session_id].Name + " -> " + com_data.line, false);
                                    client.Self.InstantMessageGroup(com_data.session_id, com_data.line);
                                }else
                                {
                                    Output_sub.Logs.add(Group_Sub.OwnGroups.groups[com_data.session_id].Name + " -> " + com_data.line, false);
                                    //client.Self.InstantMessage(
                                }
                            }
                            break;

                        case DoggyMCP.MCP_Data.GetCommand.relog:
                            DoggyServer_main.exitcode = 1;
                            client.Network.Logout();
                            break;

                        case DoggyMCP.MCP_Data.GetCommand.update_sl_client:
                            Update.get(client);
                            break;

                        case DoggyMCP.MCP_Data.GetCommand.unknown:
                            MCP_Sub.init.login(client);
                            MCP_Sub.Update.run(client);
                            Group_Sub.OwnGroups.RequestCurrentGroups(client);
                            break;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("\n" + ex.Message + "\n");
                    init.RegisterService();
                }
            }
        }

        public static void set(DoggyMCP.MCP_Data.SetCommand set_command)
        {
            if (init.use_remoting)
            {
                try
                {
                    Boolean is_nu = init.service.set_command(set_command);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("\n" + ex.Message + "\n");
                    init.RegisterService();
                }
            }
        }
    }
}
