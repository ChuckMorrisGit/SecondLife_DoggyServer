using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Simulator_Sub.Media_Sub
{
    class MusicURL
    {
        private static Dictionary<string, MusicData> parcelsURLs = new Dictionary<string, MusicData>();

        public static void init(GridClient client)
        {
            try { 
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();
            dbcmd.CommandText = "SELECT * FROM Streams;";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();


            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string simulator = reader.GetString(1);
                string parcel = reader.GetString(2);
                string url = reader.GetString(3);

                if (parcelsURLs.Keys.Contains(parcel)) parcelsURLs[parcel].urls.Add(url);
                else
                {
                    MusicData musicDataTemp = new MusicData();
                    musicDataTemp.id = id;
                    musicDataTemp.simulator = simulator;
                    musicDataTemp.parcel = parcel;
                    musicDataTemp.urls.Add(url);

                    parcelsURLs.Add(parcel, musicDataTemp);
                }
                
            }
            dbcon.Close();
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("ERROR Simulator_Sub->MusicURL: " + ex.Message, false);
            }
        }

        public static void check(string simulator, string parcel, string url)
        {
            Boolean addData = true;

            if (parcelsURLs.Keys.Contains(parcel))
                if (parcelsURLs[parcel].urls.Contains(url)) addData = false;

            if (addData)
            {
                if (parcelsURLs.Keys.Contains(parcel)) parcelsURLs[parcel].urls.Add(url);
                else
                {
                    MusicData musicDataTemp = new MusicData();
                    musicDataTemp.simulator = simulator;
                    musicDataTemp.parcel = parcel;
                    musicDataTemp.urls.Add(url);

                    parcelsURLs.Add(parcel, musicDataTemp);
                }

                if (url != "")
                {

                    IDbConnection dbcon;
                    dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                    IDbCommand dbcmd = dbcon.CreateCommand();
                    dbcon.Open();

                    string Feld = "(`Region`,`Parcel`,`URLs`)";
                    string Wert = "(\"" + simulator + "\",\"" + parcel + "\",\"" + url + "\")";

                    string sql = "INSERT IGNORE INTO `Streams` " + Feld + " VALUES " + Wert + ";";

                    dbcmd.CommandText = sql;
                    dbcmd.ExecuteNonQuery();

                    dbcon.Close();
                }
            }
        }
    }
}