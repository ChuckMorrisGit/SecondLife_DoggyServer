using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class SQL
    {
        public static void execute(string sql_command)
        {
            try
            {
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                dbcmd.CommandText = sql_command;
                dbcmd.ExecuteNonQuery();

                dbcon.Close();
            }
            catch (Exception ex)
            {
                Output_sub.Logs.add("DAtABASE UPDATE EORROR: " + ex.Message, false);
            }


        }
    }
}
