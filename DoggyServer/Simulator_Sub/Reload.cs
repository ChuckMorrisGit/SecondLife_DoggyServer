using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Simulator_Sub
{
    class Reload
    {

        public static void all(GridClient client)
        {
            DoggyServer_main.onlineTimer++;
            DoggyServer_main.simDatas = new Dictionary<string, SimData>();


            Simulator_Sub.Sim.getRegions(client);

        }

        public static void Save2Database()
        {

            IDbConnection dbcon;
            IDbCommand dbcmd;

            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            dbcmd = dbcon.CreateCommand();
            dbcon.Open();

            string sql = "TRUNCATE TABLE `Simulators`;";

            dbcmd.CommandText = sql;
            dbcmd.ExecuteNonQuery();

            string Feld;
            string Wert;

            int counter = 100;
            foreach (SimData simData in DoggyServer_main.simDatas.Values)
            {
                try
                {
                    Feld = "(`Name`,`X`,`Y`,`MapID`)";
                    Wert = "('" + simData.name + "','" + simData.xPos.ToString() + "','" + simData.yPos.ToString() + "','" + simData.mapID.ToString() + "')";
                    sql = "INSERT IGNORE INTO `Simulators` " + Feld + " VALUES " + Wert + ";";

                    dbcmd.CommandText = sql;
                    dbcmd.ExecuteNonQuery();
                }
                catch (Exception ex) { Output_sub.Logs.add("DB ERROR: " + ex.Message, false); }
                counter--;
                if (counter < 0)
                {
                    counter = 1000;
                    Output_sub.Logs.add("Sims still saving ",false);
                }
            }

            dbcon.Close();
        }
    }
}
