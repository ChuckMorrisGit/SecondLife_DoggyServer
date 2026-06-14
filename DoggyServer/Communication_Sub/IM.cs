using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Xml;
using System.IO;

namespace DoggyServer.Communication_Sub
{
    class IM
    {
        private static GridClient client_alert;
        public static Boolean alice_bot_im = false;
        public static Boolean alice_bot_groupim = false;
        enum Agent_im_status
        {
            unknow = 0,
            IM = 1,
            G_IM = 2,
        }

        public static void init(GridClient client)
        {
            client_alert = client;

            client.Self.IM += new EventHandler<InstantMessageEventArgs>(Self_IM);
            client.Self.AlertMessage += new EventHandler<AlertMessageEventArgs>(Self_AlertMessage);
        }

        public static void close(GridClient client)
        {
            client_alert = client;

            client.Self.IM -= new EventHandler<InstantMessageEventArgs>(Self_IM);
            client.Self.AlertMessage -= new EventHandler<AlertMessageEventArgs>(Self_AlertMessage);

            
        }

        static void Self_AlertMessage(object sender, AlertMessageEventArgs e)
        {
            if (!e.Message.Contains("Autopilot"))
            {
                Output_sub.Logs.add("ALERT: " + e.Message, false);
                MasterChat.reply(client_alert, e.Message);
            }
        }

        static void Self_IM(object sender, InstantMessageEventArgs e)
        {
            GridClient client = DoggyServer_main.clients[e.Simulator.Client.Self.AgentID];

            UUID fromAgentID = e.IM.FromAgentID;
            Vector3 fromPos = e.IM.Position;
            UUID fromRegionKey = e.IM.RegionID;
            uint fromRegionId = e.IM.ParentEstateID;
            System.Net.IPEndPoint ipEndPoint = e.Simulator.IPEndPoint;

            Output_sub.Logs.add("IM from UUID: " + fromAgentID.ToString(), false);
            if (Master.firstLevelMaster.Contains(fromAgentID)) Output_sub.Logs.add("Master: " + fromAgentID.ToString(), false);
            else Output_sub.Logs.add("No Master: " + fromAgentID.ToString(), false);

            //// TEMP CODE... NOCH AUSBESSERN !!!!
            Agent_im_status agent_im_status = Agent_im_status.unknow;
            if (Group_Sub.OwnGroups.groups.Keys.Contains(e.IM.IMSessionID)) agent_im_status = Agent_im_status.G_IM;
            else agent_im_status = Agent_im_status.IM;
            //// TEMP CODE... NOCH AUSBESSERN !!!!
            
            try
            {
                switch (e.IM.Dialog)
                {
                    case InstantMessageDialog.StartTyping:
                    case InstantMessageDialog.StopTyping:
                    case InstantMessageDialog.BusyAutoResponse:
                    case InstantMessageDialog.Lure911:
                        break;

                    case InstantMessageDialog.GroupNotice:
                        Output_sub.Logs.add(agent_im_status.ToString() + " -> " + e.IM.Dialog.ToString(), true);
                        break;

                    case InstantMessageDialog.RequestTeleport:
                        if (Master.firstLevelMaster.Contains(fromAgentID))
                        {
                            Movement_Sub.Follow.deleteAutoPilotPosList();
                            DoggyServer_main.scanMode = false;
                            Animation_Sub.AO.ResetAutoAnimation();
                            Movement_Sub.Stand.ava(client);
                            client.Self.TeleportLureRespond(fromAgentID, e.IM.IMSessionID, true);
                        }
                        break;

                    case InstantMessageDialog.InventoryOffered:
                        Communication_Sub.MasterChat.reply(client, "get an Item from: " + e.IM.FromAgentName + " named: " + e.IM.Message);
                        break;

                    case InstantMessageDialog.InventoryAccepted:

                        break;

                    case InstantMessageDialog.MessageFromObject:
                        Output_sub.Logs.add("O_IM -> " + Simulator_Sub.Sim.getName(client, e.IM, ipEndPoint) + " " + e.IM.FromAgentName + ": " + e.IM.Message, true);
                        MCP_Sub.Messages.im(client, e.IM.IMSessionID, e.IM.Timestamp.ToString() + ": " + e.IM.Message);
                        ObjectParser.command(client, e.IM.Message, e.IM.Position, e.IM.FromAgentID, e.IM.FromAgentName);
                        break;

                    case InstantMessageDialog.MessageFromAgent:
                    case InstantMessageDialog.SessionSend:
                        if (!DoggyServer_main.avaDatas.Keys.Contains(fromAgentID)) Avatars_Sub.NewAvatar.add2database(fromAgentID, e.IM.FromAgentName);
                        
                        if (agent_im_status == Agent_im_status.IM)
                            Parser.command(client, e.IM.Message, e.IM.Position, e.IM.FromAgentID, e.IM.FromAgentName, From.Type.IM_Ava);

                        if (agent_im_status == Agent_im_status.IM) MCP_Sub.Messages.im(client, e.IM.IMSessionID, DateTime.Now.ToString() + ": " + e.IM.FromAgentName + ": " + e.IM.Message);
                        else MCP_Sub.Messages.g_im(client, e.IM.IMSessionID, DateTime.Now.ToString() + ": " + e.IM.FromAgentName + ": " + e.IM.Message);

                        #region ALICE BOT
                        if ((alice_bot_im) && (agent_im_status== Agent_im_status.IM))
                        {
                            string chat_text = AIMLbot_sub.Alice.response(e.IM.Message, e.IM.FromAgentName);

                            client.Self.InstantMessage(fromAgentID, chat_text);

                            Output_sub.Logs.add("IM from Alice: " + chat_text, false);
                        }

                        if ((alice_bot_groupim) && (agent_im_status == Agent_im_status.G_IM))
                        {
                            if (e.IM.Message.ToLower().Contains(client.Self.FirstName.ToLower()))
                            {
                                string chat_text = AIMLbot_sub.Alice.response(e.IM.Message, e.IM.FromAgentName);

                                Output_sub.Logs.add("Group-IM from Alice: " + chat_text, false);
                                GroupIM.say(client, e.IM.IMSessionID, chat_text);
                            }
                        }
                        #endregion

                        Output_sub.Logs.add(agent_im_status.ToString() + " TEST-> " + Simulator_Sub.Sim.getName(client, e.IM, ipEndPoint) + " " + e.IM.FromAgentName + ": " + e.IM.Message, true);

                        if ((agent_im_status == Agent_im_status.G_IM) && (DoggyServer_main.agentData.type == AvaData.avaType.doggy) && (!alice_bot_groupim))
                        {
                            if (e.IM.Message.ToLower().Contains("doggy"))
                            {
                                client.Self.InstantMessageGroup(e.IM.IMSessionID, "Wuff");
                                Output_sub.Logs.add("Response in Group-IM: " + "WUFF", false);
                            }
                        }

                        //// Caprica Staff Channel
                        if (agent_im_status == Agent_im_status.G_IM)
                        {
                            Console.WriteLine("Group IM");
                            Console.WriteLine(e.IM.FromAgentID.ToString());
                        }

                        break;


                    case InstantMessageDialog.FriendshipOffered:
                        Communication_Sub.MasterChat.reply(client, e.IM.FromAgentName + " wants to be my friend: " + e.IM.Message);
                        if (DoggyServer_main.avaDatas[fromAgentID].masterLevel != 666) client.Friends.AcceptFriendship(fromAgentID, e.IM.IMSessionID);
                        break;

                    case InstantMessageDialog.MessageBox:
                        MessageBox.check(client, e.IM, e.Simulator);
                        break;

                    default:
                        //Output_sub.Logs.add("IM -> " + Simulator_Sub.Sim.getName(client, e.IM, ipEndPoint) + " " + e.IM.FromAgentName + ": " + e.IM.Message, true);
                        if (agent_im_status == Agent_im_status.IM) MCP_Sub.Messages.im(client, e.IM.IMSessionID, DateTime.Now.ToString() + ": " + e.IM.FromAgentName + ": " + e.IM.Message);
                        else MCP_Sub.Messages.g_im(client, e.IM.IMSessionID, DateTime.Now.ToString() + ": " + e.IM.FromAgentName + ": " + e.IM.Message);

                        Output_sub.Logs.add(agent_im_status.ToString() + " -> " + e.IM.Dialog.ToString(), true);
                        Output_sub.Logs.add(agent_im_status.ToString() + " -> " + e.IM.FromAgentName + ": " + e.IM.Message, true);
                        break;
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("IM ERROR: " + ex.ToString(), false); }
        }
    }
}
