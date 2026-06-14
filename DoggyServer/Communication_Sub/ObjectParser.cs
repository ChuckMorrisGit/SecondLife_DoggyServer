using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class ObjectParser
    {
        public static void command(GridClient client, string befehl, Vector3 pos, UUID fromAV, string fromName)
        {
            string[] befehlsArray = befehl.Split(' ');
            string outPutText = string.Empty;

            for (int i = 1; i < befehlsArray.Count(); i++)
            {
                outPutText += befehlsArray[i] + " ";
            }

            UUID avaKey2 = new UUID();
            UUID groupKey = new UUID();

            try
            {
                switch (befehlsArray[0].ToLower())
                {
                    case "come":
                        if (befehlsArray.Count() == 1)
                        {
                            Movement_Sub.Move.to(client, pos);
                        }
                        break;

                    case "comeandsit":
                        if (befehlsArray.Count() == 2)
                        {
                            Movement_Sub.Move.to(client, pos);
                            System.Threading.Thread.Sleep(3000);
                            Output_sub.Logs.add("Sit on: " + fromName + " - " + fromAV.ToString(), false);
                            client.Self.RequestSit(new UUID(befehlsArray[1]), Vector3.Zero);
                            client.Self.Sit();
                            Animation_Sub.AO.ResetAutoAnimation();
                        }
                        break;

                    case "givegroup":
                        if (befehlsArray.Count() == 3)
                        {
                            Group_Sub.Invide.byKey(client, new UUID(befehlsArray[1]), new UUID(befehlsArray[2]));
                        }
                        break;

                    case "checkgivegroup":
                        UUID avaKey = new UUID(befehlsArray[1]);
                        if (!DoggyServer_main.avaDatas.ContainsKey(avaKey)) Avatars_Sub.NewAvatar.addUnknown(client, avaKey);
                        Output_sub.Logs.add("TRY GROUP INVIDE: " + DoggyServer_main.avaDatas[avaKey].fullname, false);
                        if (!DoggyServer_main.avaDatas[avaKey].groupInvited)
                        {
                            if (befehlsArray.Count() == 2)
                            {
                                Group_Sub.Invide.byKey(client, avaKey, Group_Sub.Daten.caprica);
                            }
                        }
                        break;

                    case "checkgivegroupbykey":
                        avaKey2 = new UUID(befehlsArray[1]);
                        groupKey = new UUID(befehlsArray[2]);
                        if (!DoggyServer_main.avaDatas.ContainsKey(avaKey2)) Avatars_Sub.NewAvatar.addUnknown(client, avaKey2);
                        Output_sub.Logs.add("TRY GROUP INVIDE: " + DoggyServer_main.avaDatas[avaKey2].fullname, false);
                        if (!DoggyServer_main.avaDatas[avaKey2].groupInvited)
                        {
                            if (befehlsArray.Count() == 3)
                            {
                                Group_Sub.Invide.byKey(client, avaKey2, groupKey);
                            }
                        }
                        break;

                    case "givegroup4any":
                        avaKey2 = new UUID(befehlsArray[1]);
                        groupKey = new UUID(befehlsArray[2]);
                        if (!DoggyServer_main.avaDatas.ContainsKey(avaKey2)) Avatars_Sub.NewAvatar.addUnknown(client, avaKey2);
                        Output_sub.Logs.add("TRY GROUP INVIDE: " + DoggyServer_main.avaDatas[avaKey2].fullname, false);
                        Group_Sub.Invide.byKey(client, avaKey2, groupKey);
                        break;

                    case "add2marketplace":
                        InventoryItem item = new InventoryItem(new UUID(befehlsArray[1]));
                        item.Name = fromName;
                        Database_Sub.Insert2Marketplace.Insert2Item(item);
                        break;

                    case "follow":
                        Master.currentMaster = befehlsArray[1] + " " + befehlsArray[2];
                        ObjectManager_Sub.ObjectUpdate.followOn = true;
                        MasterChat.reply(client, "Follow: " + outPutText);
                        break;

                    case "stay":
                        ObjectManager_Sub.ObjectUpdate.followOn = false;
                        DoggyServer_main.scanMode = false;
                        Movement_Sub.Follow.deleteAutoPilotPosList();
                        MasterChat.reply(client, "Stay: " + fromName);
                        break;
                }
            }
            catch (Exception ex)
            {
                if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Error: " + ex.Message);
            }
        }
    }
}
