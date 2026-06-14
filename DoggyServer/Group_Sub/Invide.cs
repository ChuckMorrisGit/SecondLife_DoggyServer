using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Group_Sub
{
    class Invide
    {
        private static Random rand = new Random();

        public static void byKey(GridClient client, UUID avaKey, UUID group)
        {
            List<UUID> roles = new List<UUID>();
            roles.Add(UUID.Zero);
            client.Groups.Invite(group, roles, avaKey);

            string data_Feld = "";
            if (group == Daten.caprica) data_Feld = "GroupInvited";
            if (group == Daten.xFactor) data_Feld = "";

            if (data_Feld != "")
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                string sql = "UPDATE `Avas` SET " + data_Feld + " = 'true' WHERE `UUID` = \"" + avaKey.ToString() + "\";";

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                dbcon.Close();
            }

            Output_sub.Logs.add("GROUP INVIDE: " +DoggyServer_main.avaDatas[avaKey].fullname, false);
            DoggyServer_main.avaDatas[avaKey].groupInvited = true;
        }

        public static void selectRandom(GridClient client, UUID group)
        {
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();
            dbcmd.CommandText = "SELECT * FROM Avas WHERE GroupInvited = 'false' ORDER BY RAND();";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();

            UUID key = UUID.Zero;
            if (reader.Read())
            {
                key = UUID.Parse(reader.GetString(1));

            }
            reader.Close();
            reader.Dispose();
            dbcon.Close();

            Group_Sub.GroupMembers.membersInGroup[group].Add(key);

            if (DoggyServer_main.avaDatas.Keys.Contains(key))
            {
                Output_sub.Logs.add("GROUP INVIDE: " + DoggyServer_main.avaDatas[key].fullname, false);
                if ((!DoggyServer_main.avaDatas[key].fullname.ToLower().Contains("linden")) && (!DoggyServer_main.avaDatas[key].fullname.ToLower().Contains("ontyne")))
                {
                    Invide.byKey(client, key, UUID.Zero);
                }
            }
            else Output_sub.Logs.add("GROUP INVIDE: " + key.ToString(), false);

        }
    }
}
