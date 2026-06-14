using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenMetaverse.Packets;
using System.IO;
using System.Xml;
using System.Security.Cryptography;

namespace DoggyServer.Appearance_Sub
{
    class Shape
    {
        public static string subDir_save = "../Appearance/";
        public static string subDir_load = "../Appearance/";
        public static Boolean loadLocalVisualParams = false;
        public static Boolean checkDouble = false;
        public static Boolean deleteOld = true;
        public static int maxFileHistory_Days = 7;

        private static List<string> md5s = new List<string>();

        public static void init(GridClient client)
        {
            if (checkDouble)
            {
                string[] files = Directory.GetFiles(subDir_load);
                foreach (string file in files)
                {
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    byte[] bytes = md5.ComputeHash(stream);
                    string md5string = BitConverter.ToString(bytes).Replace("-", "");
                    if (md5s.Contains(md5string)) File.Delete(file);
                    else md5s.Add(md5string);
                }
            }

            if (deleteOld)
            {
                foreach (string file in Directory.GetFiles(subDir_save))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    try
                    {
                        TimeSpan timeSpan = DateTime.Now - fileInfo.CreationTime;
                        if (timeSpan.TotalDays > maxFileHistory_Days)
                        {
                            File.Delete(file);
                            Output_sub.Logs.add("Delete: " + fileInfo.Name, false);
                        }
                    }catch (Exception ex) { Output_sub.Logs.add("ERROR Delete: " + fileInfo.Name, false); }
                }
            }

            if (loadLocalVisualParams) loadall(client);
        }

        public static AvatarAppearancePacket searchAndLoad(GridClient client, string name)
        {
            AvatarAppearancePacket appearance = null;

            string avaName = Avatars_Sub.Search.first(client, name);
            if (avaName != string.Empty)
            {
                if (DoggyServer_main.avaNames.ContainsKey(avaName))
                {
                    UUID avaKey = DoggyServer_main.avaNames[avaName];
                    if (Appearance_Sub.Data.Appearances.ContainsKey(avaKey))
                    {
                        appearance = Appearance_Sub.Data.Appearances[avaKey];
                    }
                    else
                    {
                        string[] files = Directory.GetFiles(subDir_load);
                        string filename = string.Empty;
                        Boolean found = false;
                        foreach (string file in files)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            string avaFileName = fileInfo.Name.Split('_')[0];
                            //Output_sub.Logs.add(avaName + " - " + avaFileName,false);
                            //if ((avaFileName == avaName) && (!found))
                            if ((avaFileName == avaName) && (fileInfo.Extension == ".sla"))
                            {
                                found = true;
                                filename = file;
                            }
                        }

                        if (filename != string.Empty) appearance = load(client, filename);
                    }
                }
            }
            return (appearance);
        }

        public static void loadall(GridClient client)
        {
            string[] files = Directory.GetFiles(subDir_load);

            Output_sub.Logs.add("Read Local Visual_Param", false);

            foreach (string file in files)
            {
                try
                {
                    AvatarAppearancePacket appearance = load(client, file);

                    UUID avaKey = UUID.Zero;
                    FileInfo fileInfo = new FileInfo(file);
                    string fileName = fileInfo.Name.Split('.')[0];
                    if (DoggyServer_main.avaNames.ContainsKey(fileName))
                        avaKey = DoggyServer_main.avaNames[fileName];

                    if (avaKey != UUID.Zero)
                    {
                        lock (Appearance_Sub.Data.Appearances) Appearance_Sub.Data.Appearances[avaKey] = appearance;
                        lock (Appearance_Sub.Data.Appearance_status) Appearance_Sub.Data.Appearance_status[avaKey] = Data.appearanceStatus.localFile;
                        Output_sub.Logs.add("ADD LOCAL visual_param: " + fileName, false);
                    }
                }
                catch (Exception ex) { Output_sub.Logs.add("Read Local visual_params ERROR: " + ex.Message, false); }
            }
        }

        public static AvatarAppearancePacket load(GridClient client, string file)
        {
            AvatarAppearancePacket appearance = new AvatarAppearancePacket();
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AvatarAppearancePacket));

                FileStream fs = new FileStream(file, FileMode.Open);
                appearance = (AvatarAppearancePacket)x.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex) { Output_sub.Logs.add("Read Local visual_params ERROR: " + ex.Message, false); }
            return (appearance);
        }


        public static void save(GridClient client)
        {
            try
            {
                if (!Directory.Exists(subDir_save)) Directory.CreateDirectory(subDir_save);

                string fullname = string.Empty;

                foreach (UUID avaKey in Appearance_Sub.Data.Appearances.Keys)
                {
                    switch (Appearance_Sub.Data.Appearance_status[avaKey])
                    {
                        case Data.appearanceStatus.delete:
                            break;

                        case Data.appearanceStatus.localFile:
                            break;

                        case Data.appearanceStatus.neu:
                            writeLocalFile(avaKey);
                            break;

                        default:
                            Console.WriteLine(DoggyServer_main.avaDatas[avaKey].fullname);
                            break;
                    }
                }
            }
            catch (Exception ex) { Output_sub.Logs.add(ex.Message, false); }

        }

        private static void writeLocalFile(UUID avaKey)
        {
            try
            {
                AvatarAppearancePacket appearance = Appearance_Sub.Data.Appearances[avaKey];
                DateTime dateTime = DateTime.Now;
                string timestamp = dateTime.Year.ToString() + "_" + dateTime.Month.ToString() + "_" + dateTime.Day.ToString() + "_"
                    + dateTime.Hour.ToString() + "_" + dateTime.Minute.ToString();

                string fullname = DoggyServer_main.avaDatas[avaKey].vorname + " " + DoggyServer_main.avaDatas[avaKey].nachname;

                string filename = subDir_save + fullname + "_" + timestamp + ".sla";
                Output_sub.Logs.add("Save Appearance: " + filename, false);
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(appearance.GetType());
                TextWriter tw = new StreamWriter(filename);
                x.Serialize(tw, appearance);
                tw.Close();

                string xml_shape = AvatarAppearancePacket.ToXmlString(appearance);



                tw = new StreamWriter(filename + "_XML");
                tw.WriteLine(xml_shape);
                tw.Close();
            }
            catch (Exception ex) { Output_sub.Logs.add(DoggyServer_main.avaDatas[avaKey].fullname + ": Appearance Database Update: " + ex.Message, false); }
        }
    }
}