using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Communication_Sub.Set_Sub
{
    class Evil
    {
        public static void ToAva(GridClient client, string fullname, UUID fromAV)
        {
            List<UUID> avaKeys = DoggyServer_main.avaDatas.Keys.ToList();

            foreach (UUID avaKey in avaKeys)
            {
                if (DoggyServer_main.avaDatas[avaKey].fullname == fullname)
                {
                    DoggyServer_main.avaDatas[avaKey].masterLevel = 666;
                    client.Self.InstantMessage(fromAV, "Set Evil to: " + fullname);

                    IDbConnection dbcon;
                    dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                    IDbCommand dbcmd = dbcon.CreateCommand();
                    dbcon.Open();

                    string sql = "UPDATE `Avas` SET `MasterLevel` = '666' WHERE `UUID` = '" + avaKey.ToString() + "';";

                    dbcmd.CommandText = sql;
                    dbcmd.ExecuteNonQuery();

                    dbcon.Close();

                }
            }
        }
    }
}
