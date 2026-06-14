using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DoggyServer.Communication_Sub
{
    class MQTT
    {
        private static MqttClient client = null;
        private static string botName = "";

        public static void init(AvaData avaData)
        {
            botName = avaData.vorname + "_" + avaData.nachname;

            client = new MqttClient(System.Net.IPAddress.Parse("192.168.1.70"));

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            connenct();

        }

        private static void connenct()
        {
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.Subscribe(new string[] { "network/doggyserver/all"}, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "network/doggyserver/" + botName }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }


        public static void log2MQTT(string log)
        {

            if (!client.IsConnected)
            {
                connenct();
            }

            log = botName + ": " + log;

            client.Publish("home/logger", Encoding.UTF8.GetBytes(log), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string payload = enc.GetString(e.Message);
            string topic = e.Topic.ToString();

            switch (payload)
            {
                case "relog":
                    DoggyServer_main.running = false;
                    break;

            }
        }
    }
}
