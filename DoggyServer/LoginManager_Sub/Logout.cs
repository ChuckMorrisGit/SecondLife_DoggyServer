using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.LoginManager_Sub
{
    class Logout
    {
        public static void withItemClick(GridClient client)
        {
            Output_sub.Logs.add("LOGOUT initialisation", false);
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();
            //dbcmd.CommandText = "SELECT * FROM Boards ORDER BY RAND();";
            dbcmd.CommandText = "SELECT * FROM Boards;";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    string sim = reader.GetString(1);
                    Vector3 pos = new Vector3(reader.GetInt16(2), reader.GetInt16(3), reader.GetInt16(4));
                    uint localID = (uint)reader.GetInt16(5);

                    teleportAndClick(client, sim, pos, localID);
                }
                catch (Exception ex) { Output_sub.Logs.add(ex.Message, false); ; }
            }
            reader.Close();
            dbcon.Close();

            DoggyServer_main.exitcode = 0;
            DoggyServer_main.scanMode = false;
            Teleport_Sub.Home.go(client);
            client.Network.Logout();
        }

        private static void teleportAndClick(GridClient client, string sim, Vector3 pos, uint localID)
        {
            Teleport_Sub.Teleport.toNameAndWait(client, sim, pos);
            System.Threading.Thread.Sleep(10000);

            client.Self.Touch(localID);
        }
    }
}
