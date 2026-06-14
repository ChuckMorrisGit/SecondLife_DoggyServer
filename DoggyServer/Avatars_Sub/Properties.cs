using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Avatars_Sub
{
    class Properties
    {
        private static int counter = 0;
        public static void init(GridClient client)
        {
            client.Avatars.AvatarPropertiesReply += new EventHandler<AvatarPropertiesReplyEventArgs>(Avatars_AvatarPropertiesReply);
        }

        public static void close(GridClient client)
        {
            client.Avatars.AvatarPropertiesReply -= new EventHandler<AvatarPropertiesReplyEventArgs>(Avatars_AvatarPropertiesReply);
        }

        private static Properties_Data properties_data = null;
        public static void update(GridClient client)
        {
            try
            {
                properties_data = Database_Sub.UpdateProperties.get_item();
                if (properties_data != null)
                {
                    client.Avatars.RequestAvatarProperties(UUID.Parse(properties_data.key_string));
                }
                else
                {
                    List<UUID> avaKeys = DoggyServer_main.avaDatas.Keys.ToList();

                    Boolean found = false;
                    while ((!found) && (counter < avaKeys.Count))
                    {
                        AvaData avaData = DoggyServer_main.avaDatas[avaKeys[counter]];

                        if (avaData.rezDay == "unknow")
                        //   || (avaData.sl_pic == UUID.Zero)
                        //   || (avaData.rl_pic == UUID.Zero))
                        {
                            client.Avatars.RequestAvatarProperties(avaData.uuid);
                            found = true;
                        }
                        counter++;
                    }
                }
            }
            catch (Exception ex) { Output_sub.Logs.add("Properties ERROR: " + ex.Message, false); }
        }

        static void Avatars_AvatarPropertiesReply(object sender, AvatarPropertiesReplyEventArgs e)
        {
            DoggyServer_main.avaDatas[e.AvatarID].rezDay = e.Properties.BornOn;
            DoggyServer_main.avaDatas[e.AvatarID].sl_pic = e.Properties.ProfileImage;
            DoggyServer_main.avaDatas[e.AvatarID].rl_pic = e.Properties.FirstLifeImage;

            if ((DoggyServer_main.avasOnlineList.Contains(e.AvatarID)) && (properties_data == null))
            {
                DoggyServer_main.avaDatas[e.AvatarID].online = e.Properties.Online;
                Output_sub.Logs.add(DoggyServer_main.avaDatas[e.AvatarID].fullname + " " + e.Properties.Online.ToString(), false);
            }
            else
            {
                updateDatabase(DoggyServer_main.avaDatas[e.AvatarID]);
            }
        }

        public static void updateDatabase(AvaData avaData)
        {
            try
            {

                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                string sql = "UPDATE `Avas` SET ";

                if (properties_data == null)
                {
                    sql += "RezDay = \"" + avaData.rezDay + "\", ";
                    sql += "`SL-Picture` = \"" + avaData.sl_pic.ToString() + "\", ";
                    sql += "`RL-Picture` = \"" + avaData.rl_pic.ToString() + "\" ";
                }
                else
                {
                    switch (properties_data.command)
                    {
                        case DoggyCommands.CommandList.Update_SL_pic:
                            sql += "`SL-Picture` = \"" + avaData.sl_pic.ToString() + "\" ";
                            break;

                        case DoggyCommands.CommandList.Update_RL_pic:
                            sql += "`RL-Picture` = \"" + avaData.rl_pic.ToString() + "\" ";
                            break;

                    }
                }
                
                sql += "WHERE `UUID` = \"" + avaData.uuid + "\";";

                //Output_sub.Logs.add(sql, false);

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                Output_sub.Logs.add(avaData.fullname + " Update in database", false);

                if (properties_data != null)
                {
                    Database_Sub.UpdateProperties.set_ToDo_done(properties_data);
                    properties_data = null;
                }

                sql = "SELECT count(*) FROM Avas WHERE `RezDay` = \"unknow\";";
                dbcmd.CommandText = sql;
                dbcmd.Prepare();
                IDataReader reader;
                reader = dbcmd.ExecuteReader();

                while (reader.Read())
                {
                    Output_sub.MainScreen.missingRezDays = reader.GetInt16(0);
                }

                reader.Close();
                dbcon.Close();
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("AVA-Update ERROR: " + ex.Message, false);
            }
        }

    }

    public class Properties_Data
    {
        public string key_string = string.Empty;
        public string command = string.Empty;
        public int database_id = 0;
    }
}
