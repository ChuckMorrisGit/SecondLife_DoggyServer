using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class ReadSims
    {
        public static void all()
        {
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();
            dbcmd.CommandText = "SELECT * FROM Simulators;";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    SimData simData = new SimData();
                    simData.databaseID = reader.GetInt16(0);
                    simData.name = reader.GetString(1);
                    simData.xPos = reader.GetInt16(2);
                    simData.yPos = reader.GetInt16(3);
                    simData.mapID = UUID.Parse(reader.GetString(4));

                    DoggyServer_main.simDatas.Add(simData.name, simData);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            reader.Close();
            dbcon.Close();

        }
    }
}