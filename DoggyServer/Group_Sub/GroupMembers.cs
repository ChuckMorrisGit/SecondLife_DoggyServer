using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Group_Sub
{
    class GroupMembers
    {
        public static Dictionary<UUID, List<UUID>> membersInGroup = new Dictionary<UUID, List<UUID>>();
        private static UUID GroupRequestID;
        private static UUID sl_insideID;
        private static GridClient client_sl;
        public static List<UUID> infoAvas = new List<UUID>();
        private static Boolean auto_eject = false;

        public static void init(GridClient client)
        {
            client.Groups.GroupMembersReply += new EventHandler<GroupMembersReplyEventArgs>(Groups_GroupMembersReply);
            infoAvas.Add(new UUID("11111111-2222-3333-4444-555555555555"));
        }

        public static void ReadAllMembers(GridClient client, UUID group)
        {
            GroupRequestID = client.Groups.RequestGroupMembers(group);
        }

        public static void sl_inside(GridClient client)
        {
            client_sl = client;
            sl_insideID = client.Groups.RequestGroupMembers(Daten.sl_inside);
        }


        static void Groups_GroupMembersReply(object sender, GroupMembersReplyEventArgs e)
        {
            #region Normal
            if (e.RequestID == GroupRequestID)
            {
                string data_Feld = "";
                if (e.GroupID == Daten.caprica) data_Feld = "GroupInvited";
                if (e.GroupID == Daten.xFactor) data_Feld = "";

                Daten.groubMemberCount = 0;
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();

                foreach (UUID key in e.Members.Keys)
                {
                    try
                    {
                        Daten.groubMemberCount++;

                        if ((!membersInGroup[e.GroupID].Contains(key)) && (data_Feld != ""))
                        {
                            membersInGroup[e.GroupID].Add(key);

                            string sql = "UPDATE `Avas` SET " + data_Feld + " = 'true' WHERE `UUID` = \"" + key + "\";";

                            dbcmd.CommandText = sql;
                            dbcmd.ExecuteNonQuery();
                        }

                        string groupname = e.GroupID.ToString();
                        if (OwnGroups.groups.ContainsKey(e.GroupID)) groupname = OwnGroups.groups[e.GroupID].Name;

                        if (DoggyServer_main.avaDatas.Keys.Contains(key))
                            Output_sub.Logs.add("GROUP MEMBER: " + e.GroupID.ToString() + " " + DoggyServer_main.avaDatas[key].fullname, false);
                        else Output_sub.Logs.add("GROUP MEMBER: " + e.GroupID.ToString() + " " + key.ToString(), false);
                    }
                    catch (Exception ex) { Output_sub.Logs.add("GROUP MEMBER ERROR: " + ex.Message, false); }
                }

                dbcon.Close();
            }
            #endregion

            #region sl-inside
            UUID bad = new UUID("22222222-3333-4444-5555-666666666666"); 

            if (e.RequestID == sl_insideID)
            {
                if (client_sl.Self.Name == "Dog Ava")
                {
                    foreach (UUID key in e.Members.Keys)
                    {
                        if (key == bad)
                        {
                            client_sl.Friends.FriendList.ForEach(delegate(FriendInfo info)
                            {
                                UUID toAvaKey = info.UUID;

                                if ((infoAvas.Contains(toAvaKey)) && (info.IsOnline))
                                {
                                    client_sl.Self.InstantMessage(new UUID(toAvaKey), "Bad Ava is back in Group");

                                    if (auto_eject)
                                    {
                                        //client_sl.Groups.EjectUser(Daten.sl_inside, bad);

                                    }
                                }
                            }
                            );
                        }

                        
                    }
                }
            }
            #endregion
        }
    }
}
