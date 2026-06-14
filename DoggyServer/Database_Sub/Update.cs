using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class Update
    {
        public static void own_uuid(GridClient client)
        {
            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                string sql = "UPDATE `Accounts` SET UUID = \"" + client.Self.AgentID.ToString()
                    + "\" WHERE `Firstname` = \"" + client.Self.FirstName + "\" AND `Lastname` = \"" + client.Self.LastName + "\";";

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                dbcon.Close();
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("AVA-Update Database ERROR: " + ex.Message, false);
            }


        }
    }
}
