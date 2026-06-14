using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Communication_Sub.Mute_Sub
{
    class AddKey
    {

        public static void add(UUID avaKey)
        {
            if (!DoggyServer_main.muteListByUUID.Contains(avaKey))
            {
                DoggyServer_main.muteListByUUID.Add(avaKey);
                if (DoggyServer_main.avaDatas.ContainsKey(avaKey)) DoggyServer_main.muteListByName.Add(DoggyServer_main.avaDatas[avaKey].fullname);
                add2database(avaKey);
            }
        }
        private static void add2database(UUID uuid)
        {
            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                string Feld = "(`uuid`)";
                string Wert = "('" + uuid + "')";

                string sql = "INSERT INTO `mute` " + Feld + " VALUES " + Wert + ";";

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                dbcon.Close();
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("AVA-Update Database ERROR: " + ex.Message, false);
            }

        }

        public static void updatedatabase(UUID uuid, string fullname)
        {
            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                string sql = "UPDATE `Avas` SET Name = \"" + fullname + "\" WHERE `UUID` = \"" + uuid + "\";";

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