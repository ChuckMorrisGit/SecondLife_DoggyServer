using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Chat
    {
        private static GridClient client;
        public static Boolean chat_follow = false;
        private static DateTime last_time = DateTime.Now;
        private static List<string> antiSpanks = new List<string>();
        private static UUID Conner_Dagostino = new UUID("8cff84dc-8005-469d-bed4-f4abc5cb0b65");
        public static Dictionary<ChatHistoryData, DateTime> chatHistory = new Dictionary<ChatHistoryData, DateTime>();
        public static Boolean alice_bot = false;
        public static List<string> text_spank = new List<string>();
        public static List<string> text_titler = new List<string>();

        public static void init(GridClient client_new)
        {
            client = client_new;

            ChatRespond.init(client);
            client.Self.ChatFromSimulator += new EventHandler<ChatEventArgs>(Self_ChatFromSimulator);

            antiSpanks.Add("walter");
            antiSpanks.Add("doggy");
            antiSpanks.Add("katzerl");

            text_spank.Add("spank");
            text_spank.Add("schlägt");
            text_spank.Add("ditsch");

            text_titler.Add("title");
            text_titler.Add("titel");
            text_titler.Add("title");
            text_titler.Add("titler");
        }

        public static void close()
        {
            client.Self.ChatFromSimulator -= new EventHandler<ChatEventArgs>(Self_ChatFromSimulator);
        }

        static void Self_ChatFromSimulator(object sender, ChatEventArgs e)
        {
            ChatHistoryData chatHistoryData = new ChatHistoryData();
            chatHistoryData.from = e.FromName;
            chatHistoryData.text = e.Message;

            /*
            try
            {
                if (chatHistory.Count > 1)
                {
                    ChatHistoryData data = chatHistory.Keys.ToList()[0];
                    TimeSpan span = DateTime.Now - chatHistory[data];

                    if (span.TotalSeconds > 30) chatHistory.Remove(data);
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("CHAT HISTORY DELETE ERROR: " + ex.ToString(), false); }
            */

            try
            {
                if (!chatHistory.Keys.Contains(chatHistoryData))
                {
                    //chatHistory.Add(chatHistoryData, DateTime.Now);

                    if (e.Message != "")
                    {
                        if (e.Type == ChatType.Normal) Output_sub.Logs.add(e.FromName + " say: " + e.Message, true);
                        if (e.Type == ChatType.Shout) Output_sub.Logs.add(e.FromName + " shout: " + e.Message, true);
                        if (e.Type == ChatType.Whisper) Output_sub.Logs.add(e.FromName + " whisper: " + e.Message, true);
                        
                        MCP_Sub.Messages.chat(client, e.SourceID, DateTime.Now.ToString() + " " + e.FromName + ": " + e.Message);
                    }

                    if (!DoggyServer_main.avasOnSim.Keys.Contains(e.FromName))
                    {
                        DoggyServer_main.avasOnSim.Add(e.FromName, DoggyServer_main.avaDatas[e.OwnerID]);
                    }

                    if (e.SourceType == ChatSourceType.Agent)
                    {
                        if ((e.Position.X > 1) && (e.Position.Y > 1) && (e.Position.Z > 1))
                            DoggyServer_main.avasOnSim[e.FromName].position = e.Position;

                        if ((alice_bot) && (e.Message != ""))
                        {
                            if ((e.Message.ToLower().Contains(client.Self.FirstName.ToLower()))
                                || (AIMLbot_sub.Alice.aliceUsers.ContainsKey(e.FromName)))
                            {
                                client.Self.Movement.TurnToward(e.Position);
                                Animation_Sub.Typing.set(client, true);

                                string chat_text = AIMLbot_sub.Alice.response(e.Message, e.FromName);

                                System.Threading.Thread.Sleep(1000);
                                client.Self.Chat(chat_text, 0, ChatType.Normal);
                                Animation_Sub.Typing.set(client, false);
                            }
                        }
                    }

                    if ((!DoggyServer_main.muteListByName.Contains(e.FromName)) && (chat_follow))
                    {
                        switch (DoggyServer_main.agentData.type)
                        {
                            case AvaData.avaType.doggy:
                                if ((DateTime.Now - last_time).TotalMinutes > 3)
                                {
                                    last_time = DateTime.Now;
                                    Movement_Sub.Move.to(client, e.Position);
                                }

                                if (e.Message.ToLower().Contains("doggy"))
                                {
                                    last_time = DateTime.Now;
                                    Movement_Sub.Move.to(client, e.Position);
                                }
                                break;


                            case AvaData.avaType.catty:
                                if ((DateTime.Now - last_time).TotalMinutes > 3)
                                {
                                    last_time = DateTime.Now;
                                    Movement_Sub.Move.to(client, e.Position);
                                }
                                break;
                        }
                    }

                    if (e.SourceType == ChatSourceType.Object)
                    {
                        foreach (string text in text_titler)
                        {
                            if (e.Message.ToLower().Contains(text))
                            {
                                if (e.OwnerID == Conner_Dagostino) client.Self.Chat("KNUUURRRRR. Mir ist verboten worden unter einer drakonischen Strafe Conner einen neuen Titel zu geben", 0, ChatType.Normal);
                                else Titler.response(client, e.Message);
                            }
                        }

                        foreach (string text in text_spank)
                        {
                            if (e.Message.ToLower().Contains(text))
                            {
                                foreach (string antiSpankName in antiSpanks)
                                {
                                    if (e.Message.ToLower().Contains(antiSpankName)) Spanker.bite(client, e.Position);
                                }
                            }
                        }

                    }

                    Vector3 avaPos = e.Position;
                    Vector3 agentPos = client.Self.SimPosition;
                    Vector3d avaPos_d = new Vector3d(avaPos);
                    Quaternion qua = new Quaternion(avaPos.X, avaPos.Y, avaPos.Z);

                    client.Self.Movement.HeadRotation = qua;
                    client.Self.Movement.SendUpdate();
                    Parser.command(client, e.Message, e.Position, e.OwnerID, e.FromName, From.Type.chat);

                    ChatRespond.say(client, e);
                }
                else
                {
                    try
                    {
                        //TimeSpan span = DateTime.Now - chatHistory[chatHistoryData];

                        //if (span.TotalSeconds > 30) chatHistory.Remove(chatHistoryData);
                    }
                    catch (Exception ex) { Output_sub.Logs.add("CHATHISTORY DELETE ERROR: " + ex.ToString(), false); }

                }
            }
            catch (Exception ex) { Output_sub.Logs.add("CHAT FROM SIMULATOR ERROR: " + ex.ToString(), false); }
        }
    }

    class ChatHistoryData
    {
        public string from = string.Empty;
        public string text = string.Empty;
    }
}