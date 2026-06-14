using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Output_sub
{
    class MainScreen
    {
        //public static string avaName = "Unknown";
        public static int missingRezDays = 666;
        private static GridClient client_main;
        public static int zeilenLaenge = 100;
        private static string zeilenFueller = "";

        private static Boolean debug = false;
        private static Boolean screen_output = false;

        public static void init(GridClient client)
        {
            client_main = client;
            for (int i = 0; i < zeilenLaenge; i++) { zeilenFueller += " "; }
        }

        public static void Output(GridClient client)
        {
            if (screen_output)
            {
                if (client != null) client_main = client;
                else client = client_main;

                int counter = 0;
                Console.Clear();
                try
                {
                    string posString = "0,0,0";
                    if (client != null)
                    {
                        Vector3 pos = client.Self.SimPosition;

                        posString = Math.Round(pos.X).ToString() + ",";
                        posString += Math.Round(pos.Y).ToString() + ",";
                        posString += Math.Round(pos.Z).ToString();
                    }
                    Console.Write(Simulator_Sub.ParcelUpdate.simName + " " + posString + " - " + Simulator_Sub.ParcelUpdate.parcelName);
                    if (Simulator_Sub.ParcelUpdate.parcelProperties != null)
                    {
                        Console.Write("  Sims on CPU: " + Simulator_Sub.ParcelUpdate.parcelProperties.Simulator.CPURatio.ToString());
                        Console.Write("  AVAs: " + Simulator_Sub.ParcelUpdate.parcelProperties.Simulator.ObjectsAvatars.Count.ToString());

                        string prims = (Simulator_Sub.ParcelUpdate.parcelProperties.Parcel.OtherPrims
                            + Simulator_Sub.ParcelUpdate.parcelProperties.Parcel.OwnerPrims
                            + Simulator_Sub.ParcelUpdate.parcelProperties.Parcel.GroupPrims).ToString();
                        string maxPrims = Simulator_Sub.ParcelUpdate.parcelProperties.Parcel.MaxPrims.ToString();
                        Console.Write("  Prims: " + prims + "/" + maxPrims);
                    }
                    Console.WriteLine();

                    Console.Write("Datacenter: " + Simulator_Sub.ParcelUpdate.colo);
                    Console.Write("  " + Simulator_Sub.ParcelUpdate.parcelProperties.Simulator.SimVersion);
                    if (MCP_Sub.init.reconnect_counter > 0)
                    {
                        Console.Write("   MCP Reconnet in " + MCP_Sub.init.reconnect_counter.ToString() + " Seconds");
                    }
                    Console.WriteLine();

                    Console.Write(DoggyServer_main.agentData.type.ToString() + ": ");
                    //Console.Write(DoggyServer_main.agentData.fullname + "  On Plattform: " + DoggyServer_main.platformID.ToString() + " Traffic Mode: " + DoggyServer_main.doTraffic.ToString());
                    Console.Write(DoggyServer_main.agentData.fullname + " Access-Level:" + client.Self.AgentAccess);
                    TimeSpan time = TimeSpan.FromMinutes(DoggyServer_main.logoutCountdown);
                    TimeSpan runtime = DateTime.Now - DoggyServer_main.startTime;
                    string minstring = runtime.Minutes.ToString();
                    if (minstring.Length == 1) minstring = "0" + minstring;
                    Console.Write(" run: " + runtime.Hours.ToString() + ":" + minstring);
                    string timestring = time.ToString().Split('.')[0];
                    Console.WriteLine("  Timer:" + timestring);

                    Console.Write(Simulator_Sub.ParcelUpdate.musikURL);
                    Console.Write("   Found Sculpies: " + ObjectManager_Sub.ObjectUpdate.sculptData.Count.ToString());
                    if (Friends_Sub.Online.notification) Console.Write("   Masters ON: " + DoggyServer_main.masters_online.Count.ToString());

                    Console.Write("   " + ProgramTimer.cpu_output);

                    if (debug)
                    {
                        Console.WriteLine();
                        foreach (UUID key in DoggyServer_main.masters_online)
                        {
                            Console.WriteLine(DoggyServer_main.avaDatas[key].fullname);
                        }
                    }

                    Console.WriteLine();
                    Console.Write("  Rip-Mode: " + DoggyServer_main.avaripMode.ToString());
                    Console.Write("  Scan-Mode: " + DoggyServer_main.scanMode.ToString());
                    Console.Write("  MissRezDays: " + missingRezDays.ToString());
                    Console.Write("  ONcheck: " + DoggyServer_main.avaOnlineCheck.ToString());
                    Console.Write("  Group Invides: " + Group_Sub.GroupMembers.membersInGroup.Count.ToString());
                    Console.Write(" / " + Group_Sub.Daten.groubMemberCount.ToString());
                    Console.WriteLine();
                    Console.Write("  OnOwnParcel: " + Simulator_Sub.Parcel.CheckIfOwn(client).ToString());
                    Console.Write("  PosData: " + Movement_Sub.Autopilot.PosData.liste.Count.ToString());
                    Console.Write("  TickCounter: " + Animation_Sub.AO.tickCounter.ToString());
                    Console.Write("  Not.2do: " + Animation_Sub.AO.nothingToDo.ToString());
                    Console.Write("  Balance: " + client.Self.Balance.ToString() + "L$");
                    Console.WriteLine();

                    Console.Write("Fetching: " + Inventory_Sub.Fetch.fetching.ToString() + "   ");
                    Console.Write("Prim: ");
                    foreach (string primOwner in Simulator_Sub.ParcelObjectOwner.getOwners())
                    {
                        Console.WriteLine(primOwner + " - ");
                    }

                    Console.WriteLine();

                    Console.Write("Teleported Home: " + Movement_Sub.Autopilot.Pilot.teleported_home.ToString() + "   ");
                    Console.Write("MCP: " + MCP_Sub.init.use_remoting.ToString() + "    ");
                    Console.WriteLine(Movement_Sub.Move.moveString);

                    Console.Write("Friends online: ");
                    if (Friends_Sub.Online.notification)
                    {
                        client.Friends.FriendList.ForEach(delegate (FriendInfo info)
                        {
                            if (info.IsOnline)
                            {
                                Console.Write(": " + info.Name);

                                if (info.CanModifyMyObjects) Console.Write("O");
                                else Console.Write(".");

                                if (info.CanSeeMeOnMap) Console.Write("M");
                                else Console.Write(".");
                            }

                        }
                        );
                    }
                    else Console.Write("Ausgeschaltet");
                    Console.WriteLine();

                    Console.Write("Muted: ");
                    foreach (string avaName in DoggyServer_main.muteListByName)
                    {
                        Console.Write(avaName + ", ");
                    }
                    Console.WriteLine();

                    Console.Write("HOME: " + Simulator_Sub.Parcel.CheckIfOwn(client).ToString());
                    Console.Write("  Autopilot Able: " + Movement_Sub.Autopilot.Check.ifAble(client).ToString());
                    Console.Write("  ChatFollow: " + Communication_Sub.Chat.chat_follow.ToString());
                    Console.Write("  ERRORS: " + Logs.errorCounter.ToString());
                    Console.Write("  CHAT-HIS.: " + Communication_Sub.Chat.chatHistory.Count.ToString());
                    Console.WriteLine();
                }
                catch (Exception ex) { Console.WriteLine("Top Frame ERROR: " + ex.Message); }

                Console.Write("Chat Bot:" + Communication_Sub.Chat.alice_bot.ToString() + "   ");
                Console.Write("IM Bot:" + Communication_Sub.IM.alice_bot_im.ToString() + "   ");
                Console.Write("Group-IM Bot:" + Communication_Sub.IM.alice_bot_groupim.ToString() + "   ");
                Console.Write("Art: " + DoggyServer_main.serverArt.ToString() + "    ");
                Console.WriteLine();

                Console.WriteLine("LoginUri: " + LoginManager_Sub.Login.loginURI);

                try
                {
                    if (client != null)
                    {
                        Uri url = client.Network.CurrentSim.Caps.CapabilityURI("GetTexture");
                        Console.WriteLine("SSA-URL: " + LoginManager_Sub.Login.ssa_url);


                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }



                Console.WriteLine("-----------CHATS-----------");
                try
                {
                    while (counter < Logs.maxchatLength)
                    {
                        Console.WriteLine((Logs.chats[counter].Split('\n')[0] + zeilenFueller).Substring(0, zeilenLaenge));
                        counter++;
                    }


                }
                catch (Exception ex) { Console.WriteLine("CHATS ERROR: " + ex.Message); }

                Console.WriteLine("-----------LOGS-----------");

                try
                {
                    counter = 0;
                    while (counter < Logs.maxLogLength)
                    {
                        Console.WriteLine((Logs.logs[counter] + zeilenFueller).Substring(0, zeilenLaenge));
                        counter++;
                    }
                }
                catch (Exception ex) { Console.WriteLine("LOGS ERROR: " + ex.Message); }

            }
        }
    }
}
