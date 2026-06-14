using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class UpdateProperties
    {
        public static Avatars_Sub.Properties_Data get_item()
        {
            Avatars_Sub.Properties_Data properties = null;

            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();
            dbcmd.CommandText = "SELECT * FROM ToDo WHERE Status = '" + DoggyCommands.StatusList.toDo_new + "';";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();

            if (reader.Read())
            {
                properties = new Avatars_Sub.Properties_Data();
                properties.database_id = reader.GetInt32(0);
                properties.key_string = reader.GetString(1);
                properties.command = reader.GetString(2);
            }

            dbcon.Close();
            return (properties);
        }

        public static void set_ToDo_done(Avatars_Sub.Properties_Data properties)
        {
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();

            string sql = "UPDATE `ToDo` SET ";
            sql += "Status = \"" + DoggyCommands.StatusList.toDo_done + "\" ";
            sql += "WHERE `ID` = \"" + properties.database_id + "\";";

            dbcmd.CommandText = sql;
            dbcmd.ExecuteNonQuery();

            dbcon.Close();
        }

    }
}
