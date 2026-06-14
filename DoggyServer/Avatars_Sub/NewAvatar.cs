using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Avatars_Sub
{
    class NewAvatar
    {
        public static void init(GridClient client)
        {
            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
        }

        public static void close(GridClient client)
        {
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
        }

        private static List<UUID> unknowNames = new List<UUID>();
        static void Avatars_UUIDNameReply(object sender, UUIDNameReplyEventArgs e)
        {
            int counter = 0;
            while (counter < unknowNames.Count)
            {
                if (e.Names.ContainsKey(unknowNames[counter]))
                {
                    DoggyServer_main.avaDatas[unknowNames[counter]].fullname = e.Names[unknowNames[counter]];
                    updatedatabase(unknowNames[counter], e.Names[unknowNames[counter]]);
                }
                counter++;
            }
        }

        public static void add(GridClient client, OpenMetaverse.Avatar av)
        {
            try
            {
                AvaData avaData = new AvaData();
                DoggyServer_main.avaDatas.Add(av.ID, avaData);

                Output_sub.Logs.add(av.Name + " not in database", false);

                avaData.uuid = av.ID;
                try
                {
                    avaData.fullname = av.Name;
                    avaData.vorname = av.FirstName;
                    avaData.nachname = av.LastName;
                }
                catch (Exception ex) { }

                DoggyServer_main.avaDatas[avaData.uuid] = avaData;

                //add2database(avaData.uuid, avaData.fullname);
                Output_sub.Logs.add(av.Name + " Added to database", false);
                //client.Avatars.RequestAvatarProperties(av.ID);
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("AVA-Update ERROR: " + ex.Message, false);
            }
        }
        public static void addUnknown(GridClient client, UUID uuid)
        {
            AvaData avaData = new AvaData();
            DoggyServer_main.avaDatas.Add(uuid, avaData);

            avaData.uuid = uuid;
            avaData.vorname = "Unkown";
            avaData.nachname = "Unkown";
            avaData.fullname = avaData.vorname + " " + avaData.nachname;
            add2database(uuid, avaData.fullname);
            
            DoggyServer_main.avaDatas[avaData.uuid] = avaData;

            unknowNames.Add(uuid);
            client.Avatars.RequestAvatarProperties(uuid);
            client.Avatars.RequestAvatarName(uuid);
        }

        public static void add2database(UUID uuid, string fullname)
        {
            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                string Feld = "(`Name`,`UUID`)";
                string Wert = "('" + fullname + "','" + uuid + "')";

                string sql = "INSERT IGNORE INTO `Avas` " + Feld + " VALUES " + Wert + ";";

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