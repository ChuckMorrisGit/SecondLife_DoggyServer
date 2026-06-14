using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using OpenMetaverse;

namespace DoggyMCP.Net_Sub
{
    public class Remote
    {
        private static TcpChannel channel;
        private static int port = 9894;

        public static void init()
        {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary props = new Hashtable();
            props["port"] = port; 

            //channel = new TcpChannel(port);
            channel = new TcpChannel(props, null, provider);

            ChannelServices.RegisterChannel(channel,false);
            RemotingConfiguration.ApplicationName = "DoggyMCP";
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SimpleService), "service", WellKnownObjectMode.SingleCall);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SimpleCallback), "callback", WellKnownObjectMode.Singleton);

            Output.WriteLine("Register Remoting: " + port.ToString());
        }

        public static void close()
        {
            ChannelServices.UnregisterChannel(channel);
        }
    }

    #region Events
    public delegate void NotifyCallback(SL_Client_Sub.SL_Notify sl_notify);
    public delegate void MessageCallback(SL_Client_Sub.SL_Message message);

    public interface ICallback
    {
        event NotifyCallback Notify;
        event MessageCallback Message;
    }

    public abstract class NotifyCallbackSink : MarshalByRefObject
    {
        public void FireNotifyCallback(SL_Client_Sub.SL_Notify sl_notify)
        {
            Output.WriteLine("Activating Notify callback");
            OnNotifyCallback(sl_notify);
        }
        protected abstract void OnNotifyCallback(SL_Client_Sub.SL_Notify sl_notify);
    }

    public abstract class MessageCallbackSink : MarshalByRefObject
    {
        public void FireMessageCallback(SL_Client_Sub.SL_Message message)
        {
            Output.WriteLine("Activating Message callback");
            OnMessageCallback(message);
        }
        protected abstract void OnMessageCallback(SL_Client_Sub.SL_Message message);
    }

    public class SimpleCallback : MarshalByRefObject, ICallback
    {
        private static event NotifyCallback s_notify;
        public event NotifyCallback Notify
        {
            add { s_notify += value; Output.WriteLine("TODO : Notify init. "); }
            remove { s_notify -= value; Output.WriteLine("TODO : Notify remove."); }
        }
        public static void FireNewBroadcastedNotifyEvent(SL_Client_Sub.SL_Notify sl_notify)
        {
            Output.WriteLine("Broadcasting Notify Sending : " + sl_notify.notify_art.ToString());
            if (s_notify != null) s_notify(sl_notify);
            else Output.WriteLine("No Clients on");
        }

        private static event MessageCallback s_message;
        public event MessageCallback Message
        {
            add { s_message += value; Output.WriteLine("TODO : Message init. "); }
            remove { s_message -= value; Output.WriteLine("TODO : Message remove."); }
        }
        public static void FireNewBroadcastedMessageEvent(SL_Client_Sub.SL_Message message)
        {
            Output.WriteLine("Broadcasting Message Sending: "+  message.message);
            if (s_message != null) s_message(message);
            else Output.WriteLine("No Clients on");
        }

    }
#endregion

    #region Service
    public interface IService
    {
        Boolean log_init(UUID ava_key);
        Boolean log_add(UUID ava_key, string line);

        Boolean add_chat(UUID ava_key, UUID session_id, string line);
        Boolean add_im(UUID ava_key, UUID session_id, string line);
        Boolean add_g_im(UUID ava_key, UUID session_id, string line);

        List<Comunication_Sub.Com_data> get_chat(UUID ava_key);
        List<Comunication_Sub.Com_data> get_im(UUID ava_key);
        List<Comunication_Sub.Com_data> get_g_im(UUID ava_key);

        Boolean add_group(UUID ava_key, Comunication_Sub.Group_data group_data);
        List<Comunication_Sub.Group_data> get_groups();

        Boolean add_name(UUID ava_key, string name);
        MCP_Data.GetCommand get_command(UUID ava_key);
        Boolean set_command(MCP_Data.SetCommand set_command);

        Boolean logout(UUID ava_key);
        Boolean login(UUID ava_key);

        List<string> get_say_chat(UUID ava_key);
        List<Comunication_Sub.Com_data> get_say_im(UUID ava_key);
        Boolean set_say_chat(UUID ava_key, string line);
        Boolean set_say_IM(UUID ava_key, UUID session, string line);

        Dictionary<UUID, DoggyMCP.SL_Client_Sub.SL_Client> get_clients();

        Boolean set_SL_Client(SL_Client_Sub.SL_Client sl_Client, Boolean update_Server);
        SL_Client_Sub.SL_Client get_SL_Client(UUID ava_key);

        Boolean test(SL_Client_Sub.SL_Notify sl_notify);
    }

    public class SimpleService : MarshalByRefObject, IService
    {
        static SimpleService()
        {
            //Output.WriteLine("Init SimpleService");
        }

        public Boolean log_init(UUID ava_key)
        {
            DoggyMCP.Logs_sub.Logs.init(ava_key);
            return (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key));
        }

        public Boolean log_add(UUID ava_key, string line)
        {
            DoggyMCP.Logs_sub.Logs.add(ava_key, line);
            return (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key));
        }

        public Boolean add_chat(UUID ava_key, UUID session_id, string line)
        {
            DoggyMCP.Comunication_Sub.Add.chat(ava_key, session_id, line);
            return (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key));
        }

        public Boolean add_im(UUID ava_key, UUID session_id, string line)
        {
            DoggyMCP.Comunication_Sub.Add.im(ava_key, session_id, line);
            return (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key));
        }

        public Boolean add_g_im(UUID ava_key, UUID session_id, string line)
        {
            DoggyMCP.Comunication_Sub.Add.g_im(ava_key, session_id, line);
            return (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key));
        }

        public List<Comunication_Sub.Com_data> get_chat(UUID ava_key)
        {
            return (Comunication_Sub.Data.chats[ava_key]);
        }

        public List<Comunication_Sub.Com_data> get_im(UUID ava_key)
        {
            return (Comunication_Sub.Data.IMs[ava_key]);
        }

        public List<Comunication_Sub.Com_data> get_g_im(UUID ava_key)
        {
            return (Comunication_Sub.Data.G_IMs[ava_key]);
        }

        public Boolean add_name(UUID ava_key, string name)
        {
            SL_Client_Sub.SL_Client sl_client = new SL_Client_Sub.SL_Client();
            sl_client.name = name;
            sl_client.id = ava_key;
            sl_client.online = true;
            if (!DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key))
            {
                if (DoggyMCP_main.verbose) Output.WriteLine("ADD Client for: " + name);
                DoggyMCP.SL_Client_Sub.Data.names.Add(ava_key, sl_client);
            }

            return (true);
        }

        public Boolean add_group(UUID ava_key, Comunication_Sub.Group_data group_data)
        {
            if (!Comunication_Sub.Data.groups.ContainsKey(group_data.group_id))
            {
                Comunication_Sub.Data.groups.Add(group_data.group_id, group_data);
                if (DoggyMCP_main.verbose) Output.WriteLine("ADD Group for: " + group_data.group_name);
            }
            return (DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(ava_key));
        }

        public List<Comunication_Sub.Group_data> get_groups()
        {
            return (Comunication_Sub.Data.groups.Values.ToList());
        }

        public MCP_Data.GetCommand get_command(UUID ava_key)
        {
            MCP_Data.GetCommand command = new MCP_Data.GetCommand();
            command = MCP_Data.GetCommand.none;

            try
            {
                if (SL_Client_Sub.Data.names.ContainsKey(ava_key))
                {
                    if (Net_Sub.Commands.get.ContainsKey(ava_key))
                    {
                        if (Net_Sub.Commands.get.Count > 0)
                        {
                            if (Net_Sub.Commands.get[ava_key].Count > 0)
                            {
                                command = Net_Sub.Commands.get[ava_key][0];
                                Net_Sub.Commands.get[ava_key].RemoveAt(0);
                            }
                        }
                    }
                }
                else command = MCP_Data.GetCommand.unknown;
            }
            catch (Exception ex) { Output.WriteLine("COMMAND ERROR: " + ex.Message); }
            return (command);
        }

        public Boolean set_command(MCP_Data.SetCommand set_command)
        {
            switch (set_command)
            {
                case MCP_Data.SetCommand.restart:
                    Environment.ExitCode = 1;
                    Environment.Exit(1);
                    break;


            }

            return (true);
        }

        public Dictionary<UUID, DoggyMCP.SL_Client_Sub.SL_Client> get_clients()
        {
            return (DoggyMCP.SL_Client_Sub.Data.names);
        }

        public Boolean logout(UUID ava_key)
        {
            if (SL_Client_Sub.Data.names.ContainsKey(ava_key))
            {
                SL_Client_Sub.Data.names[ava_key].online = false;
                if (DoggyMCP_main.verbose) Output.WriteLine("LOGOUT: " + SL_Client_Sub.Data.names[ava_key].name);
            }
            SL_Client_Sub.SL_Notify sl_notify = new SL_Client_Sub.SL_Notify();
            sl_notify.notify_art = SL_Client_Sub.SL_Notify.Notify_art.LOGOUT;
            sl_notify.agent_id = ava_key;
            Net_Sub.Notify_Fire.to_all(sl_notify);
            return (true);
        }

        public Boolean login(UUID ava_key)
        {
            if (SL_Client_Sub.Data.names.ContainsKey(ava_key))
            {
                SL_Client_Sub.Data.names[ava_key].online = true;
                if (DoggyMCP_main.verbose) Output.WriteLine("LOGIN: " + SL_Client_Sub.Data.names[ava_key].name);
            }
            SL_Client_Sub.SL_Notify sl_notify = new SL_Client_Sub.SL_Notify();
            sl_notify.notify_art = SL_Client_Sub.SL_Notify.Notify_art.LOGIN;
            sl_notify.agent_id = ava_key;
            Net_Sub.Notify_Fire.to_all(sl_notify);
            return (true);
        }

        public List<string> get_say_chat(UUID ava_key)
        {
            List<string> temp = Comunication_Sub.Data.say_chat[ava_key];
            Comunication_Sub.Data.say_chat[ava_key] = new List<string>();

            return (temp);
        }

        public List<Comunication_Sub.Com_data> get_say_im(UUID ava_key)
        {
            List<Comunication_Sub.Com_data> temp = Comunication_Sub.Data.say_im[ava_key];
            Comunication_Sub.Data.say_im[ava_key] = new List<Comunication_Sub.Com_data>();

            return (temp);
        }

        public Boolean set_say_chat(UUID ava_key, string line)
        {
            if (!Comunication_Sub.Data.say_chat.ContainsKey(ava_key)) Comunication_Sub.Data.say_chat.Add(ava_key, new List<string>());
            Comunication_Sub.Data.say_chat[ava_key].Add(line);
            if (!Net_Sub.Commands.get.ContainsKey(ava_key)) Net_Sub.Commands.get.Add(ava_key, new List<MCP_Data.GetCommand>());
            Net_Sub.Commands.get[ava_key].Add(MCP_Data.GetCommand.say_chat);
            return (true);
        }

        public Boolean set_say_IM(UUID ava_key, UUID session, string line)
        {
            Comunication_Sub.Com_data com_data = new Comunication_Sub.Com_data();
            com_data.line = line;
            com_data.session_id = session;
            com_data.timestamp = DateTime.Now;

            if (!Comunication_Sub.Data.say_im.ContainsKey(ava_key)) Comunication_Sub.Data.say_im.Add(ava_key, new List<Comunication_Sub.Com_data>());
            Comunication_Sub.Data.say_im[ava_key].Add(com_data);
            if (!Net_Sub.Commands.get.ContainsKey(ava_key)) Net_Sub.Commands.get.Add(ava_key, new List<MCP_Data.GetCommand>());
            Net_Sub.Commands.get[ava_key].Add(MCP_Data.GetCommand.say_im);

            Output.WriteLine(SL_Client_Sub.Data.names[ava_key].name + " -> IM: " + line);

            return (true);
        }

        public Boolean set_SL_Client(SL_Client_Sub.SL_Client sl_Client, Boolean update_Server)
        {
            if (!DoggyMCP.SL_Client_Sub.Data.names.ContainsKey(sl_Client.id))
                DoggyMCP.SL_Client_Sub.Data.names.Add(sl_Client.id, sl_Client);
            else DoggyMCP.SL_Client_Sub.Data.names[sl_Client.id] = sl_Client;

            if (DoggyMCP_main.verbose) Output.WriteLine("UPDATE SL_CLIENT CONTROL: " + sl_Client.name);

            if (update_Server)
            {
                if (!Net_Sub.Commands.get.ContainsKey(sl_Client.id)) Net_Sub.Commands.get.Add(sl_Client.id, new List<MCP_Data.GetCommand>());
                Net_Sub.Commands.get[sl_Client.id].Add(MCP_Data.GetCommand.update_sl_client);
            }
            return (true);
        }

        public SL_Client_Sub.SL_Client get_SL_Client(UUID ava_key)
        {
            return (DoggyMCP.SL_Client_Sub.Data.names[ava_key]);
        }

        public Boolean test(SL_Client_Sub.SL_Notify sl_notify)
        {

            SimpleCallback.FireNewBroadcastedNotifyEvent(sl_notify);
            return (true);
        }
        
    }

    #endregion
}
