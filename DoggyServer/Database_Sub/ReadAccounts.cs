using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;
using System.Data;
using OpenMetaverse;

namespace DoggyServer.Database_Sub
{
    class ReadAccounts
    {
        public static Dictionary<string, Accounts> accounts = new Dictionary<string, Accounts>();

        public static void init()
        {
            IDbConnection dbcon;
            dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcon.Open();
            //dbcmd.CommandText = "SELECT * FROM Accounts ORDER BY RAND();";
            dbcmd.CommandText = "SELECT * FROM Accounts;";
            dbcmd.Prepare();
            IDataReader reader;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                Accounts account = new Accounts();

                account.id = reader.GetInt16(0);
                account.firstname = reader.GetString(1);
                account.lastname = reader.GetString(2);

                account.fullname = account.firstname + " " + account.lastname;
                account.password = reader.GetString(3);
                account.art = reader.GetString(4);

                try
                {
                    if (!reader.IsDBNull(5)) account.uuid = UUID.Parse(reader.GetString(5));
                }
                catch(Exception ex){Output_sub.Logs.add("Readaccounts ERROR: " + ex.Message,false);}
                accounts.Add(account.fullname.ToLower(), account);
            }

            dbcon.Close();
        }

        public static AvaData getFirst(string name)
        {
            AvaData avaData = new AvaData();

            Boolean found = false;

            foreach (Accounts account in accounts.Values)
            {
                if ((account.fullname.Contains(name)) && (!found))
                {
                    found = true;

                    avaData.vorname = account.firstname;
                    avaData.nachname = account.lastname;
                    avaData.password = account.password;
                }
            }

            return (avaData);
        }

        public static AvaData Menu(AvaData agent)
        {
            Console.Clear();
            Console.WriteLine("Welcher solls denn sein?\n");
            int gesammt = 0;
            Dictionary<int, Accounts> datas = new Dictionary<int, Accounts>();
            foreach (Accounts account in accounts.Values)
            {
                gesammt++;
                Console.WriteLine(gesammt.ToString() + ": " + account.fullname);
                datas.Add(gesammt, account);
            }
            Console.WriteLine("0: TO EXIT");

            Boolean found = false;

            while (!found) 
            {
                Console.Write("Eingabe: ");
                string line = Console.ReadLine();
                Console.WriteLine(line);
                int which = int.Parse(line);

                if (which == 0) Environment.Exit(0);

                if (datas.ContainsKey(which))
                {
                    agent.vorname = datas[which].firstname;
                    agent.nachname = datas[which].lastname;
                    agent.password = datas[which].password;
                    found = true;
                }
            }

            return (agent);
        }
    }
}
