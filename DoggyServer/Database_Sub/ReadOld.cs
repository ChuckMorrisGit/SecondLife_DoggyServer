using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class ReadOld
    {
        public static void all()
        {
            IDbConnection dbcon;
            IDbConnection dbcon2;
            dbcon = new MySqlConnection("SERVER=" + DoggyServer_main.mysql_server + ";DATABASE=SecondLife_Old;UID=DB_USER;PWD=DB_PASSWORD;Pooling=false");
            dbcon2 = new MySqlConnection("SERVER=" + DoggyServer_main.mysql_server + ";DATABASE=SecondLife;UID=DB_USER;PWD=DB_PASSWORD;Pooling=false");
            IDbCommand dbcmd = dbcon.CreateCommand();
            IDbCommand dbcmd2 = dbcon2.CreateCommand();
            dbcon.Open();
            dbcon2.Open();
            dbcmd.CommandText = "SELECT * FROM Giver ORDER BY RAND();";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    AvaData avaTemp = new AvaData();
                    avaTemp.fullname = reader.GetString(1);
                    string[] nameArray = avaTemp.fullname.Split(' ');
                    avaTemp.vorname = nameArray[0];
                    avaTemp.nachname = nameArray[1];

                    avaTemp.uuid = UUID.Parse(reader.GetString(2));

                    if (!DoggyServer_main.avaDatas.Keys.Contains(avaTemp.uuid))
                    {
                        DoggyServer_main.avaDatas.Add(avaTemp.uuid, avaTemp);

                        string Feld = "(`Name`,`UUID`)";
                        string Wert = "('" + avaTemp.fullname + "','" + avaTemp.uuid + "')";

                        string sql = "INSERT INTO `Avas` " + Feld + " VALUES " + Wert + ";";

                        dbcmd2.CommandText = sql;
                        dbcmd2.ExecuteNonQuery();
                        Output_sub.Logs.add(avaTemp.fullname, false);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            reader.Close();
            dbcon.Close();
            dbcon2.Close();

        }

    }
}
