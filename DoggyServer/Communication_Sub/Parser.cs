using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Communication_Sub
{
    class Parser
    {
        public static void command(GridClient client, string befehl, Vector3 pos, UUID fromAV, string fromName, From.Type fromType)
        {
            if ((DoggyServer_main.muteListByUUID.Contains(fromAV)) && (!Master.firstLevelMaster.Contains(fromAV)))
            {
                //client.Self.InstantMessage(fromAV, "Sorry you are on my Mute List. Please contact Walter Ginsburg to unmute you^^");
                return;
            }

            string[] befehlsArray = befehl.Split(' ');
            string outPutText = string.Empty;

            for (int i = 1; i < befehlsArray.Count(); i++)
            {
                outPutText += befehlsArray[i] + " ";
            }
            outPutText = outPutText.Trim();

            try
            {
                #region All Avas
                switch (befehlsArray[0].ToLower())
                {

                    case "fire":
                        //HUD_Sub.Draggo.Fire(client);
                        break;

                    case "come":
                    //case "doggy":
                    case "komm":
                        if (befehlsArray.Count() == 1)
                        {
                            Movement_Sub.Move.to(client, pos);
                            if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "come here: Done");
                        }
                        break;

                    case "stop":
                        if (befehlsArray.Count() == 1)
                            client.Self.AutoPilotCancel();
                        break;

                    case "jump":
                        Animation_Sub.AO.ResetAutoAnimation();
                        Movement_Sub.Stand.ava(client); ;
                        client.Self.Jump(true);
                        Thread.Sleep(100);
                        client.Self.Jump(false);
                        if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "Jump fired");
                        break;

                    case "goto":
                        if (befehlsArray.Count() > 1) Movement_Sub.Goto.toAva(client, befehlsArray[1]);
                        else if (DoggyServer_main.avaDatas[fromAV].lookAt != Vector3d.Zero)
                            Movement_Sub.Move.to(client, new Vector3(DoggyServer_main.avaDatas[fromAV].lookAt));
                        if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "Goto fired");
                        break;


                    case "teleport":
                        if (fromType == From.Type.IM_Ava) client.Self.SendTeleportLure(fromAV);
                        break;

                }
                #endregion

                #region Second and FirstLevelMaster
                if ((Master.secondLevelMaster.Contains(fromAV)) || (Master.firstLevelMaster.Contains(fromAV)))
                {
                    switch (befehlsArray[0].ToLower())
                    {
                        case "stand":
                            DoggyServer_main.scanMode = false;
                            Movement_Sub.Stand.ava(client);
                            break;

                        case "gohome":
                            DoggyServer_main.scanMode = false;
                            Teleport_Sub.Home.go(client);
                            break;

                        case "go":
                            if (fromType == From.Type.IM_Ava) Go.to(client, befehlsArray, fromAV, fromName);
                            break;

                        case "help":
                            Help.output(client, fromAV);
                            break;

                        case "logout":
                        case "logoff":
                            DoggyServer_main.exitcode = 0;
                            client.Network.Logout();
                            break;

                        case "relog":
                            DoggyServer_main.loginWhere = outPutText;
                            DoggyServer_main.exitcode = 2;
                            client.Network.Logout();
                            break;

                        case "rebake":
                            client.Appearance.RequestSetAppearance(true);
                            if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "Rebake fired");
                            break;

                        case "say":
                            if (fromType == From.Type.IM_Ava)
                            {
                                client.Self.Chat(outPutText, Check.ifChannel(befehlsArray[1]), ChatType.Normal);
                            }
                            break;

                        case "shout":
                            if (fromAV != UUID.Zero)
                            {
                                client.Self.Chat(outPutText, Check.ifChannel(befehlsArray[1]), ChatType.Shout);
                            }
                            break;

                        case "follow":
                            Movement_Sub.Stand.ava(client);
                            DoggyServer_main.scanMode = false;
                            if (befehlsArray.Count() == 3)
                            {
                                Master.currentMaster = befehlsArray[1] + " " + befehlsArray[2];
                            }
                            else Master.currentMaster = fromName;
                            ObjectManager_Sub.ObjectUpdate.followOn = true;

                            if (fromType == From.Type.IM_Ava)
                                client.Self.InstantMessage(fromAV, "Follow " + Master.currentMaster);
                            if (fromType == From.Type.chat)
                                client.Self.Chat("Wuff", 0, ChatType.Normal);
                            break;

                        case "stay":
                        case "sitz":
                            ObjectManager_Sub.ObjectUpdate.followOn = false;
                            DoggyServer_main.scanMode = false;
                            Movement_Sub.Follow.deleteAutoPilotPosList();
                            if (fromType == From.Type.IM_Ava)
                                client.Self.InstantMessage(fromAV, "Follow " + "Stay here");
                            if (fromType == From.Type.chat)
                                client.Self.Chat("Stay here", 0, ChatType.Normal);
                            break;

                        case "show":
                            if (fromType == From.Type.IM_Ava) Status.checkAndPrint(client, befehlsArray[1], fromAV);
                            break;

                        case "scan":
                            DoggyServer_main.scanMode = true;
                            client.Self.InstantMessage(fromAV, "Scan Mode " + DoggyServer_main.scanMode.ToString());
                            break;

                        case "check":
                            if (fromType == From.Type.IM_Ava) Check.what(client, befehlsArray, fromAV, fromName);
                            break;

                        case "clean":
                            Animation_Sub.Play.doggyClean(client);
                            break;
                    }
                }
                #endregion

                #region FirstLevelMaster
                if (Master.firstLevelMaster.Contains(fromAV))
                {
                    switch (befehlsArray[0].ToLower())
                    {
                        case "set":
                            if (fromType == From.Type.IM_Ava) Set.to(client, befehlsArray, fromAV, fromName);
                            break;

                        case "inv":
                        case "inventory":
                            if (fromType == From.Type.IM_Ava) Inventory.to(client, befehlsArray, fromAV, fromName);
                            break;

                        case "ao":
                            if (fromType == From.Type.IM_Ava) AO.set(client, befehlsArray, fromAV, fromName);
                            break;

                        case "outfit":
                            if (fromType == From.Type.IM_Ava) Outfit.set(client, befehlsArray, fromAV, fromName);
                            break;

                        case "touch":
                            lock (client.Network.Simulators)
                            {
                                for (int i = 0; i < client.Network.Simulators.Count; i++)
                                {
                                    client.Network.Simulators[i].ObjectsPrimitives.ForEach(
                                        delegate(Primitive prim)
                                        {
                                            if (prim.ID == DoggyServer_main.avaDatas[fromAV].pointAt_id)
                                            {
                                                client.Self.Touch(prim.LocalID);
                                            }
                                        }
                                   );
                                }
                            }
                            break;

                        case "sit":
                            Chat.chat_follow = false;
                            ObjectManager_Sub.ObjectUpdate.followOn = false;
                            if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "sit on: " + DoggyServer_main.avaDatas[fromAV].pointAt_id.ToString());
                            Movement_Sub.Sit.onLocalPrim(client, DoggyServer_main.avaDatas[fromAV].pointAt_id);
                            break;

                        case "wp":
                            //Movement_Sub.WayPoint.add(fromAV, pos);
                            if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "WayPoint Added");
                            break;

                        case "balance":
                            if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "I have " + client.Self.Balance.ToString() + "L$");
                            break;

                        case "give":
                            if (fromType == From.Type.IM_Ava)
                            {
                                int amount = int.Parse(befehlsArray[1]);
                                if (amount > client.Self.Balance) client.Self.InstantMessage(fromAV, "I have only " + client.Self.Balance.ToString() + "L$");
                                else client.Self.GiveAvatarMoney(fromAV, amount);
                            }
                            break;

                        case "getprim":
                            lock (client.Network.Simulators)
                            {
                                for (int i = 0; i < client.Network.Simulators.Count; i++)
                                {
                                    client.Network.Simulators[i].ObjectsPrimitives.ForEach(
                                        delegate(Primitive prim)
                                        {
                                            if (prim.ID == DoggyServer_main.avaDatas[fromAV].pointAt_id)
                                            {
                                                ObjectManager_Sub.Prim.get(client, prim.LocalID);
                                            }
                                        }
                                   );
                                }
                            }

                            break;

                        case "findprim":
                            if (fromType == From.Type.IM_Ava) ObjectManager_Sub.FindObejct.perName(client, outPutText, fromAV);
                            break;

                        //MOVEMENTS
                        case "fw":
                        case "forwart":
                            if (fromType == From.Type.IM_Ava) Movement_Sub.Goto.forward(client, fromAV, float.Parse(befehlsArray[1]));
                            break;

                        case "tt":
                        case "turnto":
                            if (fromType == From.Type.IM_Ava)
                                Movement_Sub.Goto.turnTo(client, fromAV, new Vector3(float.Parse(befehlsArray[1]), float.Parse(befehlsArray[2]), 0));
                            break;

                        case "tl":
                        case "turnleft":
                            break;

                        case "tr":
                        case "turnright":
                            break;

                        case "gettexture":
                            Asset_Sub.Image.get(client, "TEXTURE", UUID.Parse(befehlsArray[1]));
                            if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "Get Texture:" + befehlsArray[1]);
                            break;

                        case "save":
                            Appearance_Sub.Shape.save(client);
                            break;

                        case "fetch":
                            Inventory_Sub.Fetch.all(client, fromAV, true);
                            break;

                        case "groupim":
                            UUID groupkey = UUID.Parse(befehlsArray[1]);
                            string outPutText_IM = string.Empty;

                            for (int i = 2; i < befehlsArray.Count(); i++)
                            {
                                outPutText_IM += befehlsArray[i] + " ";
                            }
                            string error = GroupIM.say(client, groupkey, outPutText_IM);
                            if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "Test OK: " + error);
                            break;

                        case "send":
                            if (fromType == From.Type.IM_Ava) Send.to(client, befehlsArray, fromAV, fromName);
                            break;

                        case "play":
                            Play.command(client, befehlsArray, fromAV, fromName);
                            break;

                        case "mute":
                            if (fromType == From.Type.IM_Ava) Mute.to(client, befehlsArray, fromAV, fromName);
                            break;

                        case "petmeeroo":
                        case "testhud":
                        case "hudtest":
                            HUD_Sub.Meeroos.pet(client, fromAV);
                            break;

                        case "try":
                            if (fromType == From.Type.IM_Ava) Try.something(client, befehlsArray, fromAV, fromName);
                            break;

                        case "bell":
                            Sound_Sub.Play.bellen(client);
                            break;

                        case "add":
                            if (fromType == From.Type.IM_Ava) Add.to(client, befehlsArray, fromAV, fromName);
                            break;

                        case "backup":
                            switch (befehlsArray[1].ToLower())
                            {
                                case "set":
                                    ObjectManager_Sub.Copy.GetPrimSet.whole(client, fromAV, true);
                                    break;
                            }
                            break;

                        case "givefullperm":
                            client.Friends.GrantRights(fromAV, FriendRights.CanSeeOnMap | FriendRights.CanSeeOnline | FriendRights.CanModifyObjects);
                            break;

                        default:
                            //if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "Unknown Command");
                            break;
                    }
                }
                #endregion
            }

            catch (Exception ex)
            {
                if (fromType == From.Type.IM_Ava) client.Self.InstantMessage(fromAV, "Error: " + ex.Message);
            }
        }
    }
}