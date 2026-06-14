using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using OpenMetaverse;

namespace DoggyServer.MCP_Sub
{
    class init
    {
        public static Boolean use_remoting = false;
        public static Boolean mcp_restart = false;
        public static TcpChannel channel;
        public static DoggyMCP.Net_Sub.IService service;
        public static string doggyMCP_url = "localhost:9894";
        //public static string doggyMCP_url = "webserver.home:9894";
        public static int reconnect_counter = 0;
        

        public static void all()
        {
            try
            {
                if (use_remoting)
                {
                    RegisterService();
                }
            }catch (Exception ex)
            {
                Output_sub.Logs.add("MCP REGISTER ERROR_ " + ex.Message, false);
                Environment.Exit(1);
            }

        }

        public static void RegisterService()
        {

            try
            {
                channel = null;
                channel = new TcpChannel(0);
                ChannelServices.RegisterChannel(channel, false);
                service = (DoggyMCP.Net_Sub.IService)Activator.GetObject(typeof(DoggyMCP.Net_Sub.IService), "tcp://" + doggyMCP_url + "/DoggyMCP/service");
            }
            catch (Exception e)
            {
                Output_sub.Logs.add("MCP Remote ERROR: " + e.Message, false);
                use_remoting = false;
                reconnect_counter = 3;
            }

        }

        public static void ConnectStatus()
        {
            if (reconnect_counter > 0)
            {
                reconnect_counter--;

                if (reconnect_counter == 0) use_remoting = true;
            }
        }

        public static void init_mcp(GridClient client)
        {
            if (use_remoting)
            {
                try
                {

                    Output_sub.Logs.add("Add Client to MCP", false);
                    service.add_name(client.Self.AgentID, client.Self.Name);
                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("MCP Add Client ERROR: " + ex.Message, false);
                    init.RegisterService();
                }
            }
        }

        public static void logout(GridClient client)
        {
            if (use_remoting)
            {
                try
                {
                    Output_sub.Logs.add("LOGOUT to MCP", false);
                    service.logout(client.Self.AgentID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n" + ex.Message + "\n");
                    init.RegisterService();
                }
            }
        }

        public static void login(GridClient client)
        {
            if (use_remoting)
            {
                try
                {
                    Output_sub.Logs.add("LOGIN to MCP", false);
                    service.login(client.Self.AgentID);
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
