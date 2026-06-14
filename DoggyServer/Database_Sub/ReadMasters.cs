using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Packets;
using MySqlConnector;
using System.Data;

namespace DoggyServer.Database_Sub
{
    class ReadMasters
    {
        private static Boolean readAppearance = false;
        private static List<UUID> doubleNames = new List<UUID>();
        public static Boolean deleteDouble = false;
        private static List<string> deleteNames = new List<string>();

        public static void all()
        {
            try
            {
                Output_sub.Logs.add("try 2 Connect to Database: " + DoggyServer_main.mysql_server, false);
                IDbConnection dbcon;
                dbcon = new MySqlConnection(SecretsManager.GetMySqlConnectionString());
                IDbCommand dbcmd = dbcon.CreateCommand();
                dbcon.Open();
                dbcmd.CommandText = "SELECT * FROM Avas ORDER BY RAND();";
                dbcmd.Prepare();
                Output_sub.Logs.add("Connected to Database", false);
                IDataReader reader;
                reader = dbcmd.ExecuteReader();

                while (reader.Read())
                {
                    AvaData avaTemp = new AvaData();
                    try
                    {
                        avaTemp.fullname = reader.GetString(0);
                        string[] nameArray = avaTemp.fullname.Split(' ');
                        if (nameArray.Count() < 2) nameArray = avaTemp.fullname.Split('.');

                        avaTemp.vorname = nameArray[0];
                        avaTemp.nachname = nameArray[1];

                        avaTemp.uuid = UUID.Parse(reader.GetString(1));
                        if (!reader.IsDBNull(2)) avaTemp.rezDay = reader.GetString(2);
                        if (!reader.IsDBNull(3)) avaTemp.sl_pic = UUID.Parse(reader.GetString(3));
                        if (!reader.IsDBNull(4)) avaTemp.rl_pic = UUID.Parse(reader.GetString(4));
                        if (!reader.IsDBNull(5)) avaTemp.masterLevel = reader.GetInt16(5);

                        if (!reader.IsDBNull(7))
                        {
                            if (reader.GetString(7) == "true")
                            {
                                avaTemp.inSacriGroup = true;
                                //Group_Sub.GroupMembers.membersInGroup.Add(avaTemp.uuid);
                            }
                        }

                        if (!reader.IsDBNull(11))
                        {
                            if (reader.GetString(11) == "true")
                            {
                                avaTemp.groupInvited = true;
                                Group_Sub.GroupMembers.membersInGroup[Group_Sub.Daten.caprica].Add(avaTemp.uuid);
                            }
                        }

                        //if (reader.GetString(12) == "true")
                        //{
                        //    avaTemp.xfactorInvited = true;
                        //    Group_Sub.GroupMembers.membersInGroup[Group_Sub.Daten.xFactor].Add(avaTemp.uuid);
                        //}

                        if (reader.GetInt16(8) != 0)
                        {
                            avaTemp.online_check = true;
                            DoggyServer_main.avasOnlineList.Add(avaTemp.uuid);
                        }

                        DoggyServer_main.avaDatas.Add(avaTemp.uuid, avaTemp);
                        if (!DoggyServer_main.avaNames.ContainsKey(avaTemp.fullname))
                            DoggyServer_main.avaNames.Add(avaTemp.fullname, avaTemp.uuid);
                        else
                        {
                            //Output_sub.Logs.add("Double Name: " + avaTemp.fullname, false);
                            doubleNames.Add(avaTemp.uuid);
                        }

                        if (avaTemp.masterLevel < 99)
                        {
                            Master.masters.Add(avaTemp.fullname, avaTemp);
                            Output_sub.Logs.add("ADD MASTER: " + avaTemp.fullname, false);

                            switch (avaTemp.masterLevel)
                            {
                                case 1:
                                    Master.firstLevelMaster.Add(avaTemp.uuid);
                                    Output_sub.Logs.add("ADD FirstLevelMaster: " + avaTemp.fullname, false);
                                    break;

                                case 2:
                                    Master.secondLevelMaster.Add(avaTemp.uuid);
                                    Output_sub.Logs.add("ADD SecondLevelMaster: " + avaTemp.fullname, false);
                                    break;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Output_sub.Logs.add(avaTemp.fullname + ": Ava Database Read Error: " + ex.Message, false);
                        deleteNames.Add(avaTemp.fullname);
                    }

                    if (readAppearance)
                    {
                        try
                        {
                            if ((!reader.IsDBNull(9)) && (!reader.IsDBNull(10)) && (avaTemp.uuid != UUID.Zero))
                            {
                                List<AvatarAppearancePacket.VisualParamBlock> visual_params = new List<AvatarAppearancePacket.VisualParamBlock>();

                                string blockString = reader.GetString(9);
                                string[] blockArray = blockString.Split('|');

                                foreach (string stringValue in blockArray)
                                {
                                    AvatarAppearancePacket.VisualParamBlock visual_param = new AvatarAppearancePacket.VisualParamBlock();
                                    if (byte.TryParse(stringValue, out visual_param.ParamValue))
                                    {
                                        visual_params.Add(visual_param);
                                    }
                                }

                                List<byte> textureEntry = new List<byte>();
                                string textString = reader.GetString(10);
                                string[] textArray = textString.Split('|');

                                foreach (string textValue in textArray)
                                {
                                    byte textByte;
                                    if (byte.TryParse(textValue, out textByte))
                                    {
                                        textureEntry.Add(textByte);
                                    }
                                }
                                AvatarAppearancePacket packet = new AvatarAppearancePacket();
                                packet.VisualParam = visual_params.ToArray();
                                packet.ObjectData.TextureEntry = textureEntry.ToArray();
                                if (!Appearance_Sub.Data.Appearances.ContainsKey(avaTemp.uuid)) Appearance_Sub.Data.Appearances.Add(avaTemp.uuid, packet);
                                if (!Appearance_Sub.Data.Appearance_status.ContainsKey(avaTemp.uuid)) Appearance_Sub.Data.Appearance_status.Add(avaTemp.uuid, Appearance_Sub.Data.appearanceStatus.inDatabase);

                            }

                        }
                        catch (Exception ex)
                        {
                            Appearance_Sub.Data.Appearance_status[avaTemp.uuid] = Appearance_Sub.Data.appearanceStatus.delete;
                            Output_sub.Logs.add(avaTemp.fullname + ": Database Appearance Read Error: " + ex.Message, false);
                        }
                    }
                }
                reader.Close();

                foreach (string deleteName in deleteNames)
                {
                    Output_sub.Logs.add("DELETE FROM DATABASE: " + deleteName, false);
                    dbcmd.CommandText = "DELETE FROM Avas WHERE Name = '" + deleteName + "';";
                    dbcmd.ExecuteNonQuery();
                }


                if (deleteDouble)
                {
                    foreach (UUID avaKey in doubleNames)
                    {
                        dbcmd.CommandText = "DELETE FROM Avas WHERE UUID = '" + avaKey.ToString() + "';";
                        dbcmd.ExecuteNonQuery();
                    }
                }

                dbcon.Close();
            }catch(Exception ex)
            {
                Output_sub.Logs.add("ERROR ReadMasters: " + ex.ToString(), false);
            }
        }
    }
}