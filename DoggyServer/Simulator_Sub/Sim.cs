using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Simulator_Sub
{
    class Sim
    {
        public static Dictionary<ulong, string> region_names = new Dictionary<ulong, string>();
        private static int simCounter = 0;

        public static void init(GridClient client)
        {
            try { 
            client.Grid.GridRegion += new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
            client.Grid.RegionHandleReply += new EventHandler<RegionHandleReplyEventArgs>(Grid_RegionHandleReply);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Simulator_Sub->Sim: " + ex.Message, false);
            }
        }

        public static void close(GridClient client)
        {
            
            client.Grid.GridRegion -= new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
            client.Grid.RegionHandleReply -= new EventHandler<RegionHandleReplyEventArgs>(Grid_RegionHandleReply);

        }

        static void Grid_RegionHandleReply(object sender, RegionHandleReplyEventArgs e)
        {

        }

        public static void getRegions(GridClient client)
        {
            Database_Sub.SQL.execute("DELETE FROM Simulators;");
            client.Grid.RequestMainlandSims(GridLayerType.Objects);
            simCounter = 0;
        }

        static void Grid_GridRegion(object sender, GridRegionEventArgs e)
        {
            try
            {

                if (!region_names.ContainsKey(e.Region.RegionHandle))
                {
                    simCounter++;
                    if (simCounter > 1000)
                    {
                        simCounter = 0;
                        Output_sub.Logs.add("Get " + DoggyServer_main.simDatas.Count.ToString() + " SIM " + e.Region.Name, false);
                    }

                    region_names.Add(e.Region.RegionHandle, e.Region.Name);

                    SimData simData = new SimData();
                    simData.name = e.Region.Name;
                    simData.mapID = e.Region.MapImageID;
                    simData.xPos = e.Region.X;
                    simData.yPos = e.Region.Y;

                    add2database(simData);

                    if (!DoggyServer_main.simDatas.ContainsKey(simData.name)) DoggyServer_main.simDatas.Add(simData.name, simData);

                    if (DoggyServer_main.onlineTimer - (DateTime.Now - DoggyServer_main.startTime).TotalMinutes < 3)
                        DoggyServer_main.onlineTimer++;

                }
            }
            catch (Exception ex) { Output_sub.Logs.add(ex.Message, false); }

            //DoggyServer_main.DoLogout();
        }

        private static List<SimData> simData_adds = new List<SimData>();
        private static System.Threading.Thread add2database_thread;
        private static Boolean add2database_running = false;
        private static void add2database(SimData simdata)
        {
            simData_adds.Add(simdata);

            if (!add2database_running)
            {
                add2database_running = true;
                add2database_thread = new System.Threading.Thread(new System.Threading.ThreadStart(add2database_run));
                add2database_thread.IsBackground = true;
                add2database_thread.Start();
                
            }
        }

        private static void add2database_run()
        {
            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                while (simData_adds.Count > 0)
                {
                    SimData simdata = simData_adds[0];
                    string name = simdata.name.Replace("'", "_").Replace("\"","_");

                    try
                    {
                        string Feld = "(`Name`,`X`,`Y`,`MapID`)";
                        string Wert = "('" + name + "','" + simdata.xPos.ToString() + "','" + simdata.yPos.ToString() + "','" + simdata.mapID.ToString() + "')";

                        string sql = "INSERT IGNORE INTO `Simulators` " + Feld + " VALUES " + Wert + ";";

                        dbcmd.CommandText = sql;
                        dbcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) { Output_sub.Logs.add("INSERT SIM ERROR-DATA: " + ex.ToString(), false); }
                    simData_adds.Remove(simdata);
                }
                dbcon.Close();
            }
            catch (Exception ex) { Output_sub.Logs.add("INSERT SIM ERROR: " + ex.ToString(),false); }


            add2database_running = false;
        }

        public static string getRandomName()
        {
            Random rand = new Random();
            int which = rand.Next(DoggyServer_main.simDatas.Keys.Count());

            List<string> names = DoggyServer_main.simDatas.Keys.ToList();
            string name = names[which];

            Output_sub.Logs.add("Random Login Sim: " + name, false);

            return (name);
        }

        public static string getName(GridClient client, InstantMessage im, System.Net.IPEndPoint ipEndPoit)
        {
            //string name = "Unkown";



            Simulator sim = client.Network.FindSimulator(ipEndPoit);
            string name = sim.Name;

            //if (im.ParentEstateID != 0) name += " " + im.ParentEstateID.ToString();

            //if (im.RegionID != UUID.Zero) name += " " + im.RegionID.ToString();

            //if (region_names.ContainsKey(im.ParentEstateID)) name = region_names[im.ParentEstateID];

            return (name);
        }
    }
}