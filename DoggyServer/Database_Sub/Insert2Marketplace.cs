using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class Insert2Marketplace
    {
        public static void Insert2Item(InventoryItem item)
        {
            Output_sub.Logs.add("TRY ADD 2 MARKETPLACE: " + item.Name, false);

            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                string Feld = "(`Name`,`UUID`,`added`)";
                string Wert = "('" + item.Name + "','" + item.AssetUUID + "','" + String.Format("{0:s}", DateTime.Now) + "')";

                string sql = "INSERT IGNORE INTO `Marketplace` " + Feld + " VALUES " + Wert + ";";

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                dbcon.Close();
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ADD 2 MARKETPLACE ERROR: " + ex.Message, false);
            }


        }
    }
}
