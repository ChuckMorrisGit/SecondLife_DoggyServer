using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Communication_Sub.Mute_Sub
{
    class ReadMuteList
    {
        public static void all(GridClient client)
        {
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();
            dbcmd.CommandText = "SELECT * FROM mute;";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    UUID avaKey = UUID.Parse(reader.GetString(1));

                    if (avaKey != client.Self.AgentID)
                    {
                        DoggyServer_main.muteListByUUID.Add(avaKey);

                        if (DoggyServer_main.avaDatas.ContainsKey(avaKey))
                            DoggyServer_main.muteListByName.Add(DoggyServer_main.avaDatas[avaKey].fullname);
                        else
                            DoggyServer_main.muteListByName.Add(avaKey.ToString());
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            reader.Close();
            dbcon.Close();

        }

    }
}