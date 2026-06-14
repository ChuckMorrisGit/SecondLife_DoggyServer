using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

//#xcopy *.* \\phoenix.home\apps\DoggyServer\ /E /Y 

namespace DoggyServer
{
    class DoggyServer_main
    {
        public enum ServerArt
        {
            normal = 0,
            homeset = 1,
            textureNC = 2,
            rake = 3,
        }

        #region Variables

        public static int channel_hud = 5314619;
        public static Boolean debug_hud = false;
        public static Boolean debug = false;
        public static int exitcode = 1;
        public static int waitForSeconds = 1;
        public static Dictionary<UUID, AvaData> avaDatas = new Dictionary<UUID, AvaData>();
        public static Dictionary<string, UUID> avaNames = new Dictionary<string, UUID>();
        public static Dictionary<string, AvaData> avasOnSim = new Dictionary<string, AvaData>();
        public static Dictionary<string, SimData> simDatas = new Dictionary<string, SimData>();
        public static Dictionary<UUID, GridClient> clients = new Dictionary<UUID, GridClient>();
        public static List<UUID> muteListByUUID = new List<UUID>();
        public static List<string> muteListByName = new List<string>();
        public static List<UUID> avasOnlineList = new List<UUID>();
        public static Boolean avaOnlineCheck = false;
        public static Boolean scanMode = false;
        public static Boolean avaripMode = false;
        public static Boolean updateProperties = false;
        private static GridClient client;
        public static AvaData agentData = new AvaData();
        public static Boolean groupInvites = false;
        public static PlatformID platformID;
        public static Boolean doTraffic = false;
        public static string mysql_server = "127.0.0.1";
        public static string loginWhere = string.Empty;
        public static Boolean save_Appearance = false;
        public static Boolean save_Animation = false;
        public static Boolean save_Animation_NC = false;
        public static Boolean getMetaData = false;
        public static int onlineTimer = -99;
        private static Boolean ontineTimerRelog = false;
        private static DateTime endTime = DateTime.Now;
        public static DateTime startTime = DateTime.Now;
        public static double logoutCountdown = 0;
        public static Boolean joinGroup = false;
        public static UUID joinGroupID = UUID.Zero;
        private static Random rand = new Random();
        public static Boolean running = true;
        public static List<UUID> masters_online = new List<UUID>();
        public static Boolean PrimRechterAnMaster = false;
        public static Boolean doHomeTP = true;
        public static Boolean reloadAllSims = false;
        public static Boolean getAllSims = false;
        public static ServerArt serverArt = ServerArt.normal;
        #endregion

        static void Main(string[] args)
        {
            if (true)
            //if (PID.check(args))
                {
                try
                {
                    Console.WriteLine("Starting Doggy Server");

                    // Initialisiere SecretsManager zum Laden aller sensiblen Daten
                    SecretsManager.Initialize();
                    mysql_server = SecretsManager.MySqlServer;

                    //System.Threading.Thread.Sleep(10000);

                    MCP_Sub.init.all();

                    Variables_Sub.init.all();

                    serverArt = ServerArt.normal;
                    startTime = DateTime.Now;
                    endTime = startTime;

                    Output_sub.init.all(client);
                    Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

                    Database_Sub.init.all();

                    PublicSims.init();

                    platformID = Environment.OSVersion.Platform;

                    Master.init();


                    if (DoggyServer_main.platformID.ToString() != "Unix")
                    {
                        agentData.vorname = "Doggy";
                        agentData.nachname = "Ghostraven";
                        agentData.password = SecretsManager.DefaultAvatarPassword;
                    }

                    #region Commandline Abfrage
                    int argsCounter = 0;
                    Boolean maybe_help = false;

                    while (argsCounter < args.Count())
                    {

                        if ((argsCounter + 1) < args.Count())
                        {
                            if (args[argsCounter + 1] == "help") maybe_help = true;
                            else maybe_help = false;
                        }

                        switch (args[argsCounter])
                        {
                            case StartParams.firstname:
                                if (maybe_help) StartParams.firstname_help();
                                argsCounter++;
                                agentData.vorname = args[argsCounter];
                                break;

                            case StartParams.lastname:
                                if (maybe_help) StartParams.lastname_help();
                                argsCounter++;
                                agentData.nachname = args[argsCounter];
                                break;

                            case StartParams.password:
                            case StartParams.passwort:
                                if (maybe_help) StartParams.password_help();
                                argsCounter++;
                                agentData.password = args[argsCounter];
                                break;

                            case StartParams.avatype:
                                #region AvaType
                                if (maybe_help) StartParams.password_help();
                                argsCounter++;
                                switch (args[argsCounter])
                                {
                                    case "male":
                                        agentData.type = AvaData.avaType.male;
                                        break;

                                    case "female":
                                        agentData.type = AvaData.avaType.female;
                                        break;

                                    default:
                                        Console.WriteLine("Unkown AvaType: " + args[argsCounter]);
                                        Environment.Exit(0);
                                        break;
                                }
                                break;
                                #endregion

                            #region Gridauswahl
                            case StartParams.grid:
                                if (maybe_help) StartParams.grid_help();
                                argsCounter++;
                                switch (args[argsCounter])
                                {
                                    case "main":
                                        LoginManager_Sub.Login.loginURI = LoginManager_Sub.Grids.sl_main;
                                        break;

                                    case "beta":
                                        LoginManager_Sub.Login.loginURI = LoginManager_Sub.Grids.sl_beta;
                                        break;

                                    case "phoenix":
                                        LoginManager_Sub.Login.loginURI = LoginManager_Sub.Grids.phoenix;
                                        break;

                                    default:
                                        Console.WriteLine("Unkown Grid: " + args[argsCounter]);
                                        Environment.Exit(0);
                                        break;
                                }
                                break;
                            #endregion

                            case StartParams.joingroup:
                                if (maybe_help) StartParams.joingroup_help();
                                argsCounter++;
                                joinGroupID = new UUID(args[argsCounter]);
                                joinGroup = true;
                                break;

                            case StartParams.timer:
                                if (maybe_help) StartParams.timer_help();
                                argsCounter++;
                                onlineTimer = int.Parse(args[argsCounter]);
                                endTime = startTime.AddSeconds(onlineTimer);
                                break;

                            case StartParams.endtimer:
                                if (maybe_help) StartParams.endtimer_help();
                                argsCounter++;
                                if (!DateTime.TryParse(args[argsCounter], out endTime)) endTime = startTime;
                                break;

                            case StartParams.fromdatebase:
                                if (maybe_help) StartParams.fromdatebase_help();
                                argsCounter++;
                                agentData = Database_Sub.ReadAccounts.getFirst(args[argsCounter]);
                                break;

                            case StartParams.loginsim:
                                if (maybe_help) StartParams.loginsim_help();
                                argsCounter++;
                                loginWhere = args[argsCounter];
                                break;

                            case StartParams.randomSim:
                                if (maybe_help) StartParams.randomSim_help();
                                loginWhere = Simulator_Sub.Sim.getRandomName();
                                break;

                            case StartParams.getallSims:
                                if (maybe_help) StartParams.getallSims_help();
                                getAllSims = true;
                                break;
                                
                            case StartParams.noHomeTP:
                                if (maybe_help) StartParams.noHomeTP_help();
                                doHomeTP = false;
                                break;

                            case StartParams.follow:
                                if (maybe_help) StartParams.follow_help();
                                ObjectManager_Sub.ObjectUpdate.followOn = true;
                                break;

                            case StartParams.chatfollow:
                                if (maybe_help) StartParams.choiceava_help();
                                Communication_Sub.Chat.chat_follow = true;
                                break;

                            case StartParams.public_:
                                if (maybe_help) StartParams.public_help();
                                LoginManager_Sub.Login.startPublicArea = true;
                                break;

                            case StartParams.scan:
                                if (maybe_help) StartParams.scan_help();
                                scanMode = true;
                                break;

                            case StartParams.rip:
                                if (maybe_help) StartParams.rip_help();
                                avaripMode = true;
                                break;

                            case StartParams.update:
                                if (maybe_help) StartParams.update_help();
                                updateProperties = true;
                                break;

                            case StartParams.findobjekt:
                                if (maybe_help) StartParams.findobjekt_help();
                                ObjectManager_Sub.ObjectUpdate.objectUpdate = true;
                                break;

                            case StartParams.groupinvide:
                                if (maybe_help) StartParams.groupinvide_help();
                                groupInvites = true;
                                break;

                            case StartParams.dotraffic:
                                if (maybe_help) StartParams.dotraffic_help();
                                doTraffic = true;
                                break;

                            case StartParams.checkonline:
                                if (maybe_help) StartParams.checkonline_help();
                                avaOnlineCheck = true;
                                break;

                            case StartParams.last:
                                if (maybe_help) StartParams.last_help();
                                loginWhere = "last";
                                break;

                            case StartParams.home:
                                if (maybe_help) StartParams.home_help();
                                loginWhere = "home";
                                break;

                            case StartParams.deletepid:
                                if (maybe_help) StartParams.deletepid_help();
                                PID.ending();
                                break;

                            case StartParams.homeset:
                                if (maybe_help) StartParams.homeset_help();
                                serverArt = ServerArt.homeset;
                                break;

                            case StartParams.marketplace:
                                if (maybe_help) StartParams.marketplace_help();
                                serverArt = ServerArt.textureNC;
                                break;

                            case StartParams.showfriends:
                                if (maybe_help) StartParams.showfriends_help();
                                Friends_Sub.Online.notification = true;
                                break;

                            case StartParams.lookdatabase:
                                if (maybe_help) StartParams.lookdatabase_help();
                                agentData = Database_Sub.ReadAccounts.getFirst(args[argsCounter]);
                                break;

                            case StartParams.choiceava:
                                if (maybe_help) StartParams.choiceava_help();
                                agentData = Database_Sub.ReadAccounts.Menu(agentData);
                                break;

                            case StartParams.reloadSimDatabase:
                                if (maybe_help) StartParams.reloadSimDatabase_help();
                                reloadAllSims = true;
                                break;

                            case StartParams.noFetchInventory:
                                if (maybe_help) StartParams.noFetchInventory_help();
                                Inventory_Sub.init.fetchInvetory = false;
                                break;

                            case StartParams.alice_chat:
                                Communication_Sub.Chat.alice_bot = true;
                                break;

                            case StartParams.alice_im:
                                Communication_Sub.IM.alice_bot_im = true;
                                break;

                            case StartParams.alice_groupim:
                                Communication_Sub.IM.alice_bot_groupim = true;
                                break;

                            case StartParams.use_remoting:
                                Console.WriteLine("MCP Obsolet");
                                //Environment.Exit(1);
                                //MCP_Sub.init.use_remoting = true;
                                break;

                            case StartParams.mcp_restart:
                                MCP_Sub.init.mcp_restart = true;
                                break;

                            case StartParams.rake:
                                #region Rake Params
                                serverArt = ServerArt.rake;

                                Boolean ende = false;
                                while (((argsCounter + 1) < args.Count()) && (!ende))
                                {
                                    switch (args[argsCounter + 1])
                                    {
                                        case "objects":
                                        case "objekts":
                                            Inventory_Sub.Rake_Sub.Rake.rake_objects = true;
                                            argsCounter++;
                                            break;

                                        default:
                                            ende = true;
                                            break;
                                    }
                                }

                                break;

                                #endregion

                            default:
                                Console.WriteLine("Unkown Command: " + args[argsCounter]);
                                Environment.Exit(0);
                                break;
                        }
                        argsCounter++;
                    }
                    #endregion

                    Console.WriteLine(">" + agentData.vorname + " " + agentData.nachname + "<");
                    Console.WriteLine(">" + agentData.nachname + "<");
                    Console.WriteLine(">" + agentData.password + "<");

                    // Environment.Exit(0);


                    if ((agentData.vorname != string.Empty) && (agentData.nachname != string.Empty))
                    {

                        agentData.fullname = agentData.vorname + " " + agentData.nachname;

                        Communication_Sub.MQTT.init(agentData);

                        #region Art selector
                        if (Database_Sub.ReadAccounts.accounts.ContainsKey(agentData.fullname.ToLower()))
                        {
                            switch (Database_Sub.ReadAccounts.accounts[agentData.fullname.ToLower()].art)
                            {
                                case "dog":
                                    agentData.type = AvaData.avaType.doggy;
                                    break;

                                case "cow":
                                    agentData.type = AvaData.avaType.cow;
                                    break;

                                case "male":
                                    agentData.type = AvaData.avaType.male;
                                    break;

                                case "horse":
                                    agentData.type = AvaData.avaType.horse;
                                    break;

                                case "tiger":
                                    agentData.type = AvaData.avaType.catty;
                                    break;

                                default:
                                    agentData.type = AvaData.avaType.female;
                                    break;
                            }
                        }
                        #endregion

                        #region Login Routine
                        //Output_sub.Logs.add("Current Master: " + Master.currentMaster, false);
                        client = new GridClient();

                        try
                        {
                            Communication_Sub.MQTT.log2MQTT("login");
                            client = LoginManager_Sub.Login.doggy(agentData);
                        }
                        catch (Exception ex) 
                        { 
                            Output_sub.Logs.add("MAIN ERROR: " + ex.Message, false);
                            Environment.Exit(0);
                        }

                        try
                        {
                            Avatars_Sub.Properties.update(client);
                        }
                        catch (Exception ex) 
                        { 
                            Output_sub.Logs.add("AVATAR PROPERTIES ERROR: " + ex.Message, false);
                        }

                        //Avatars_Sub.Search.all(client);

                        if (endTime != startTime)
                        {
                            onlineTimer = (int)(endTime - startTime).TotalMinutes;
                        }
                        else
                        {
                            onlineTimer = 240 + rand.Next(60);
                            ontineTimerRelog = true;
                        }

                        string command = string.Empty;
                        int invideCounter = 10;
                        int doTrafficCounter = 1200000 + rand.Next(200000);
                        if (doTraffic) doTrafficCounter = 120;
                        #endregion

                        #region MAIN LOOP
                        if (reloadAllSims) Simulator_Sub.Reload.all(client);

                        int updateCounter = 0;

                        if (serverArt == ServerArt.rake) Inventory_Sub.Rake_Sub.Rake.run(client);

                        //check Mesh Assets
                        Opensim_Sub.AssetCheck.getAKFEfiles(client);

                        while ((client.Network.Connected) && (running))
                        {
                            try
                            {
                                if (debug) Output_sub.Logs.add("Serverart: " + serverArt.ToString(), false);
                                switch (serverArt)
                                {
                                    case ServerArt.normal:
                                        #region Normal server
                                        System.Threading.Thread.Sleep(500);

                                        if (getMetaData) Media_sub.Metadata.getAndSet(client);

                                        if (scanMode) Scanner_Sub.Scan.sims(client);
                                        if (updateProperties)
                                        {
                                            updateCounter--;
                                            if (updateCounter < 0)
                                            {
                                                updateCounter = rand.Next(50) + 10;
                                                Avatars_Sub.Properties.update(client);
                                            }
                                        }

                                        if (invideCounter <= 0)
                                        {
                                            //if (groupInvites) Group_Sub.Invide.selectRandom(client);
                                            invideCounter = 10;
                                        }

                                        if (avaOnlineCheck)
                                        {
                                            Avatars_Sub.OnlineCheck.CheckNext();
                                        }

                                        if (doTrafficCounter <= 0)
                                        {
                                            exitcode = 1;
                                            Output_sub.Logs.add("doTrafficCounter < 0", false);
                                            client.Network.Logout();
                                        }
                                        invideCounter--;
                                        doTrafficCounter--;

                                        if (onlineTimer != -99)
                                        {
                                            logoutCountdown = onlineTimer - (DateTime.Now - startTime).TotalMinutes;

                                            if ((masters_online.Count > 0) && (logoutCountdown < 3) && (agentData.type == AvaData.avaType.doggy)) onlineTimer++;

                                            if (logoutCountdown <= 0)
                                            {
                                                Output_sub.Logs.add("logoutCountdown < 0", false);
                                                client.Network.Logout();
                                                if (ontineTimerRelog) exitcode = 1;
                                                else exitcode = 0;
                                                Output_sub.LogFile.close(client);
                                            }
                                        }

                                        Simulator_Sub.Forbit.check(client);


                                        break;
                                        #endregion

                                    case ServerArt.rake:
                                        #region Rake server
                                        System.Threading.Thread.Sleep(500);

                                        if (getMetaData) Media_sub.Metadata.getAndSet(client);

                                        if (scanMode) Scanner_Sub.Scan.sims(client);
                                        if (updateProperties)
                                        {
                                            updateCounter--;
                                            if (updateCounter < 0)
                                            {
                                                updateCounter = rand.Next(50) + 10;
                                                Avatars_Sub.Properties.update(client);
                                            }
                                        }

                                        if (invideCounter <= 0)
                                        {
                                            //if (groupInvites) Group_Sub.Invide.selectRandom(client);
                                            invideCounter = 10;
                                        }

                                        invideCounter--;
                                        doTrafficCounter--;
                                        
                                        Simulator_Sub.Forbit.check(client);

                                        break;
                                        #endregion

                                    case ServerArt.homeset:
                                        #region Homepos setzen
                                        if (Simulator_Sub.ParcelUpdate.simName != "Grand Hustle") client.Self.Teleport(new UUID("f47887d8-ff1f-ee32-dde6-507f8ebf8b66"));
                                        //Movement_Sub.Move.toAndWait(client, new Vector3(199, 72, 48));

                                        System.Threading.Thread.Sleep(2000);

                                        client.Self.RequestSit(new UUID("10facbbf-f950-92ce-b2d6-82efaea16380"), Vector3.Zero);
                                        //282280729
                                        client.Self.Sit();
                                        System.Threading.Thread.Sleep(2000);

                                        client.Self.SetHome();
                                        exitcode = 0;

                                        System.Threading.Thread.Sleep(5000);
                                        client.Self.Chat("Done", 0, ChatType.Normal);

                                        client.Network.Logout();
                                        #endregion
                                        break;

                                    case ServerArt.textureNC:
                                        Inventory_Sub.Textures.toTXT(client);
                                        exitcode = 0;
                                        client.Network.Logout();
                                        break;

                                    default:
                                        Output_sub.Logs.add("Don't Know what to do^^", false);
                                        break;
                                }
                            }
                            catch (Exception ex) { 
                                Output_sub.Logs.add("MAIN LOOP ERROR: " + ex.Message, false);
                                running = false;

                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    Output_sub.Logs.add("MAIN LOOP ERROR: " + ex.Message, false);
                    Console.WriteLine(ex.Message + "\n");
                }

                Communication_Sub.MQTT.log2MQTT("logout");


                Output_sub.Logs.add("Running: " + running.ToString(), false);

                MCP_Sub.init.logout(client); 
                if (client.Network.Connected) client.Network.Logout();
                if (reloadAllSims) Simulator_Sub.Reload.Save2Database();

                Output_sub.LogFile.close(client);
                
                if (save_Appearance) Appearance_Sub.Shape.save(client);
                if (save_Animation) Animation_Sub.Rip.save(client);
                if (save_Animation_NC) Asset_Sub.Ani.saveAnis2NC(client.Self.Name);

                try{
                    Asset_Sub.close.all(client);
                    Animation_Sub.close.all(client);
                    Appearance_Sub.close.all(client);
                    Avatars_Sub.close.all(client);
                    ObjectManager_Sub.close.all(client);
                    Communication_Sub.close.all(client);
                    Simulator_Sub.Sim.close(client);
                    ProgramTimer.close();
                }
                catch(Exception ex){Console.WriteLine("ERROR CLOSE: " + ex.Message);}

                //if ((exitcode != 0) && (loginWhere == "last")) exitcode = 2;
                //if ((exitcode != 0) && (loginWhere == "home")) exitcode = 3;



                Console.WriteLine("Ending PID");
                PID.ending();
                Console.WriteLine("Set Exit Code");
                Environment.ExitCode = exitcode;
                Console.WriteLine("EXIT!!!");
                                
                Environment.Exit(exitcode);
            }
            else Console.WriteLine("I am running\n\n");
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            DoLogout();
        }

        public static void DoLogout()
        {
            exitcode = 0;
            PID.ending();
            Environment.ExitCode = exitcode;
            client.Network.Logout();
        }
    }
}
