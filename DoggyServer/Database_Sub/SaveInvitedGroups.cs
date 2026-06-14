using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class SaveInvitedGroups
    {
        public static void all()
        {
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();

            foreach (UUID groupKey in Group_Sub.GroupMembers.membersInGroup.Keys)
            {
                foreach (UUID avaKey in Group_Sub.GroupMembers.membersInGroup[groupKey])
                {
                    string Feld = "(`ava`,`group`)";
                    string Wert = "('" + avaKey.ToString() + "','" + groupKey.ToString() + "')";

                    string sql = "INSERT IGNORE INTO `InvitedGroups` " + Feld + " VALUES " + Wert + ";";

                    dbcmd.CommandText = sql;
                    dbcmd.ExecuteNonQuery();
                }
            }
            dbcon.Close();
        }
    }
}
