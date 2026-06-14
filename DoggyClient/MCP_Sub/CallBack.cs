using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels.Tcp;
using OpenMetaverse;

namespace DoggyClient.MCP_Sub
{
    class CallBack
    {
        private static NotifySink notify_sink = new NotifySink();
        private static MessageSink message_sink = new MessageSink();

        public static void init()
        {
            notify_sink = new NotifySink();
            MCP_Sub.init.callback.Notify += new DoggyMCP.Net_Sub.NotifyCallback(notify_sink.FireNotifyCallback);

            message_sink = new MessageSink();
            MCP_Sub.init.callback.Message += new DoggyMCP.Net_Sub.MessageCallback(message_sink.FireMessageCallback);
        }

        public static void close()
        {
            MCP_Sub.init.callback.Notify -= new DoggyMCP.Net_Sub.NotifyCallback(notify_sink.FireNotifyCallback);

            MCP_Sub.init.callback.Message -= new DoggyMCP.Net_Sub.MessageCallback(message_sink.FireMessageCallback);
        }
    }

    class NotifySink : DoggyMCP.Net_Sub.NotifyCallbackSink
    {
        protected override void OnNotifyCallback(DoggyMCP.SL_Client_Sub.SL_Notify sl_notify)
        {
            switch (sl_notify.notify_art)
            {
                case DoggyMCP.SL_Client_Sub.SL_Notify.Notify_art.LOGIN:
                case DoggyMCP.SL_Client_Sub.SL_Notify.Notify_art.LOGOUT:
                    MCP_Sub.init.get_clients_all();
                    MainForm_Sub.Clients.Fill_Clients();
                    break;
            }
        }
    }

    class MessageSink : DoggyMCP.Net_Sub.MessageCallbackSink
    {
        protected override void OnMessageCallback(DoggyMCP.SL_Client_Sub.SL_Message message)
        {
            Comunikation_Sub.Messages.add_Message_Line(message);
        }
    }
}
