using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using OpenMetaverse;

namespace DoggyClient.MCP_Sub
{
    class init
    {
        public static TcpChannel channel;
        public static DoggyMCP.Net_Sub.IService service;
        public static DoggyMCP.Net_Sub.ICallback callback;
        public static string doggyMCP_url = "192.168.1.100:9894";
        //public static string doggyMCP_url = "webserver.home:9894";
        public static Dictionary<UUID, DoggyMCP.SL_Client_Sub.SL_Client> names = new Dictionary<UUID, DoggyMCP.SL_Client_Sub.SL_Client>();

        public static void all()
        {
            RegisterService();

            MCP_Sub.CallBack.init();
        }

        public static void RegisterService()
        {
            try
            {
                channel = new TcpChannel(0);
                ChannelServices.RegisterChannel(channel, false);
                service = (DoggyMCP.Net_Sub.IService)Activator.GetObject(typeof(DoggyMCP.Net_Sub.IService), "tcp://" + doggyMCP_url + "/DoggyMCP/service");
                callback = (DoggyMCP.Net_Sub.ICallback)Activator.GetObject(typeof(DoggyMCP.Net_Sub.ICallback), "tcp://" + doggyMCP_url + "/DoggyMCP/callback");

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(NotifySink), "ServerEvents", WellKnownObjectMode.Singleton);
                NotifySink sink = new NotifySink();
            }
            catch (Exception e)
            {

            }
        }

        public static void get_clients_all()
        {
            try
            {
                names = service.get_clients();

                foreach (UUID key in names.Keys)
                {
                    Comunikation_Sub.Messages.get_history(key);
                }
            }
            catch (Exception ex)
            {
                init.RegisterService();
            }
        }
    }
}

