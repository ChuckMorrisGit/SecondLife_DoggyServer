using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Avatars_Sub
{
    class Search
    {
        private static GridClient client;
        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Directory.DirPeopleReply += new EventHandler<DirPeopleReplyEventArgs>(Directory_DirPeopleReply);
        }

        public static void close(GridClient client_new)
        {
            client = client_new;
            client.Directory.DirPeopleReply -= new EventHandler<DirPeopleReplyEventArgs>(Directory_DirPeopleReply);
        }

        static void Directory_DirPeopleReply(object sender, DirPeopleReplyEventArgs e)
        {
            foreach (DirectoryManager.AgentSearchData data in e.MatchedPeople)
            {
                string fullname = data.FirstName + " " + data.LastName;
                Output_sub.Logs.add("AVA FOUND: " + fullname, false);

                if (!DoggyServer_main.avaDatas.Keys.Contains(data.AgentID))
                {
                    AvaData avaData = new AvaData();
                    avaData.uuid = data.AgentID;
                    avaData.fullname = fullname;
                    DoggyServer_main.avaDatas.Add(data.AgentID, avaData);
                    Avatars_Sub.NewAvatar.add2database(data.AgentID, fullname);
                }
            }
        }

        public static void all(GridClient client)
        {
            client.Directory.StartPeopleSearch("linden", 0);
        }

        private static System.Threading.AutoResetEvent waitQuery = new System.Threading.AutoResetEvent(false);
        private static string result = string.Empty;
        public static string first(GridClient client, string name)
        {
            result = string.Empty;
            waitQuery.Reset();
            client.Directory.DirPeopleReply += Directory_DirPeople;

            client.Directory.StartPeopleSearch(name, 0);
            if (waitQuery.WaitOne(20000, false) && client.Network.Connected)
            {
                Output_sub.Logs.add("Find a Ava:" + result, false);
            }
            else
            {
                Output_sub.Logs.add("no Ava found for " + name, false);
            }

            client.Directory.DirPeopleReply -= Directory_DirPeople;
            return result;
        }

        private static void Directory_DirPeople(object sender, DirPeopleReplyEventArgs e)
        {
            if (e.MatchedPeople.Count > 0)
            {
                result=e.MatchedPeople[0].FirstName + " " + e.MatchedPeople[0].LastName;
                Output_sub.Logs.add("Find a Ava Reply:" + result, false);
            }
            waitQuery.Set();
        }
    }
}
