using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Packets;
//using OpenMetaverse.Utilities;

namespace DoggyServer.LoginManager_Sub
{
    public class Login
    {
        public static GridClient client = new GridClient();
        public static Boolean startPublicArea = true;
        public static Boolean useTorproxy = false;
        public static Boolean addMasterFriend = false;
        public static Boolean loginMasterLog = true;
        public static Boolean requestAllFriends = false;
        public static string ssa_url = "Not Set";
        public static string loginURI = Grids.sl_main;

        public static GridClient doggy(AvaData avaData)
        {
            client = new GridClient();
            try
            {
                LSL_Sub.init.all(client);
                Movement_Sub.init.all(client);
                Communication_Sub.Init.all(client);
                ObjectManager_Sub.init.all(client);
                Animation_Sub.init.all(client);
                Movement_Sub.Autopilot.Pilot.init(client);

                Simulator_Sub.init.all(client);

                Grid_Sub.FindMaster.init(client);

                Friends_Sub.init.all(client);

                Avatars_Sub.init.all(client);

                Asset_Sub.init.all(client);

                Appearance_Sub.init.all(client);

                Teleport_Sub.init.all(client);

                Group_Sub.init.all(client);

                Map_Sub.init.all(client);

                Backup_Sub.init.all(client);

                Media_sub.init.all(client);

                Profile_Sub.init.all(client);

                Sound_Sub.init.all(client);

                if (DoggyServer_main.platformID.ToString() != "Unix") ViewerLogFile_Sub.init.initall(client);

                client.Settings.LOG_RESENDS = true;
                client.Settings.LOG_ALL_CAPS_ERRORS = true;

                client.Settings.STORE_LAND_PATCHES = true;

                client.Settings.ALWAYS_DECODE_OBJECTS = true;
                client.Settings.ALWAYS_REQUEST_OBJECTS = true;

                client.Settings.SEND_AGENT_UPDATES = true;
                client.Settings.SEND_AGENT_THROTTLE = true;
                client.Settings.SEND_AGENT_APPEARANCE = true;
                client.Settings.DISABLE_AGENT_UPDATE_DUPLICATE_CHECK = true; //NEW

                client.Settings.FETCH_MISSING_INVENTORY = true;
                client.Settings.HTTP_INVENTORY = true;
                client.Settings.USE_ASSET_CACHE = true;
                
                client.Settings.ENABLE_SIMSTATS = true;
                client.Settings.OBJECT_TRACKING = true;
                client.Settings.MULTIPLE_SIMS = false;

                client.Settings.USE_HTTP_TEXTURES = true;

                client.Settings.ENABLE_CAPS = true;

                

                //client.Settings.USE_LLSD_LOGIN = true;

                //client.


                client.Network.RegisterCallback(PacketType.AgentDataUpdate, AgentDataUpdateHandler);
                client.Network.RegisterCallback(PacketType.AvatarAppearance, AvatarAppearanceHandler);
                //client.Network.RegisterCallback(PacketType.AlertMessage, AlertMessageHandler);

                LoginParams loginParams = new LoginParams();

                if (DoggyServer_main.groupInvites)
                {
                    loginParams = client.Network.DefaultLoginParams(avaData.vorname, avaData.nachname, avaData.password, "Test", "ver 1.0");
                    loginParams.Platform = "Windows XP";
                }
                else
                {
                    loginParams = client.Network.DefaultLoginParams(avaData.vorname, avaData.nachname, avaData.password, "DoggySLClient", "0.1 Alpha");
                    loginParams.Platform = "Mono under Linux console";
                }

                switch (DoggyServer_main.loginWhere)
                {
                    case "":
                        loginParams.Start = "home";
                        break;

                    case "home":
                    case "last":
                        loginParams.Start = DoggyServer_main.loginWhere;
                        break;

                    default:
                        loginParams.Start = OpenMetaverse.NetworkManager.StartLocation(DoggyServer_main.loginWhere, 128, 128, 128);
                        break;
                }

                client.Network.Disconnected += new EventHandler<DisconnectedEventArgs>(Network_Disconnected);
                client.Network.LoginProgress += new EventHandler<LoginProgressEventArgs>(Network_LoginProgress);

                loginParams.AgreeToTos = true;
                loginParams.URI = loginURI;

                if (useTorproxy)
                {
                    
                }

                Output_sub.Logs.add("Try Login", false);

                if (client.Network.Login(loginParams))
                {
                    Output_sub.Logs.add("Logged in: " + avaData.fullname, false);
                    try
                    {
                        ssa_url = client.Network.AgentAppearanceServiceURL;

                        MCP_Sub.init.init_mcp(client);

                        MCP_Sub.init.login(client);

                        MCP_Sub.Update.run(client);

                        Communication_Sub.MasterChat.reply(client, "Version: " + Environment.Version.Major.ToString());

                        RunParameter.set(client);
                        if (MCP_Sub.init.mcp_restart) MCP_Sub.Command.set(DoggyMCP.MCP_Data.SetCommand.restart);

                        DoggyServer_main.clients.Add(client.Self.AgentID, client);

                        Output_sub.Logs.add("Connected", false);
                        client.Self.RequestBalance();
                        Animation_Sub.Set.on(client);


                        ProgramTimer.init(client);

                        if (DoggyServer_main.getAllSims) Simulator_Sub.Sim.getRegions(client);

                        Group_Sub.Set.normal(client);

                        if (DoggyServer_main.debug_hud) client.Self.Chat("DEBUG;ON", DoggyServer_main.channel_hud, ChatType.Normal);
                        Inventory_Sub.init.all(client);

                        if (requestAllFriends) Friends_Sub.Online.requestAll(client);

                        HUD_Sub.init.all(client);

                        if (DoggyServer_main.joinGroup) Group_Sub.Join.club(client);

                        Group_Sub.GroupMembers.ReadAllMembers(client, Group_Sub.Daten.caprica);

                        Communication_Sub.Mute_Sub.ReadMuteList.all(client);

                        Group_Sub.GroupMembers.ReadAllMembers(client, Group_Sub.Daten.xFactor);

                        if (addMasterFriend) Friends_Sub.AddFriend.master(client);

                        Friends_Sub.AddFriend.OtherBots(client);

                        Group_Sub.GroupMembers.sl_inside(client);

                        /*
                        if ((startPublicArea) && (avaData.type != AvaData.avaType.female) 
                            && (avaData.type != AvaData.avaType.male)
                            && (DoggyServer_main.loginWhere != "last"))
                        {
                            Simulator_Sub.Forbit.check(client);
                            System.Threading.Thread.Sleep(15000);
                            Teleport_Sub.Home.go(client);
                        }
                        */

                        Communication_Sub.MasterChat.reply(client, "Ich have " + client.Self.Balance.ToString() + "L$###");



                        Communication_Sub.MasterChat.reply(client, "Fetching Invetory");
                        Inventory_Sub.Fetch.all(client, UUID.Zero, false);

                        if ((DoggyServer_main.loginWhere != "last") && (DoggyServer_main.doHomeTP))
                        {
                            System.Threading.Thread.Sleep(1500);
                            Teleport_Sub.Home.go(client);
                        }


                        if (Database_Sub.ReadAccounts.accounts.Keys.Contains(client.Self.Name.ToLower()))
                        {
                            if (Database_Sub.ReadAccounts.accounts[client.Self.Name.ToLower()].uuid == UUID.Zero)
                            {
                                Database_Sub.Update.own_uuid(client);
                            }
                        }


                        Communication_Sub.MasterChat.reply(client, "Init Done");
                    }
                    catch (Exception ex)
                    {
                        Communication_Sub.MasterChat.reply(client, ex.Message);
                        Output_sub.Logs.add("Login ERROR: " + ex.Message, false);
                    }

                }
                else
                {
                    Output_sub.Logs.add("can't login", false);
                }
            }
            catch(Exception ex) {Output_sub.Logs.add("LOGIN ERROR: " + ex.Message, false); }

            return (client);
        }

        static void Network_LoginProgress(object sender, LoginProgressEventArgs e)
        {
            Output_sub.Logs.add("Login Progress: ", false);
            Output_sub.Logs.add("Message: " + e.Message, false);
            Output_sub.Logs.add("Status: " + e.Status, false);
            Output_sub.Logs.add("FailReason: " + e.FailReason, false);

            Output_sub.Logs.add("Login string: " + DoggyServer_main.loginWhere,false);

            if (e.FailReason == "key") Environment.Exit(0);
            
        }

        static void Network_Disconnected(object sender, DisconnectedEventArgs e)
        {
            Output_sub.Logs.add("DisConnected", false);
        }

        private static void AvatarAppearanceHandler(object sender, PacketReceivedEventArgs e)
        {
            Packet packet = e.Packet;

            AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;

            

            //lock (Appearances) Appearances[appearance.Sender.ID] = appearance;
        }

        private static void AgentDataUpdateHandler(object sender, PacketReceivedEventArgs e)
        {
            AgentDataUpdatePacket p = (AgentDataUpdatePacket)e.Packet;

            Output_sub.Logs.add("Agent Update: " + p.AgentData.FirstName.ToString() + " " + p.AgentData.LastName.ToString(), false);

            
        }
    }
}
