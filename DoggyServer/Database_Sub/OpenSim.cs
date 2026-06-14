using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class OpenSim
    {
        public static void updateAssetData (UUID asset_UUID, byte[] asset_data)
        {
            try
            {

                Output_sub.Logs.add("UPDATE OPENSIM ASSET DATABASE: " + asset_UUID.ToString(), false);
                string mysql_string = SecretsManager.GetOpenSimConnectionString();

                IDbConnection dbcon;
                dbcon = new MySqlConnection(mysql_string);
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                dbcmd.CommandText = "UPDATE `assets` SET data = ?data WHERE id = \"" + asset_UUID.ToString() + "\";";

                MySqlParameter dataParameter = new MySqlParameter("?data", MySqlDbType.Blob, asset_data.Length);

                dataParameter.Value = asset_data;

                dbcmd.Parameters.Add(dataParameter);

                dbcmd.ExecuteNonQuery();

                dbcon.Close();
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR UPDATE OPENSIM ASSET DATABASE: " + asset_UUID.ToString(), false);
            }
        }
    }
}
