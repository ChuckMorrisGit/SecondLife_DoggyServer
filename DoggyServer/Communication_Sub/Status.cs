using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Status
    {
        public static void checkAndPrint(GridClient client, string command, UUID fromAV)
        {
            switch (command)
            {
                case "master":
                    client.Self.InstantMessage(fromAV, "Current Master: " + Master.currentMaster);
                    break;

                case "masters":
                    foreach (UUID master in Master.firstLevelMaster)
                        client.Self.InstantMessage(fromAV, "1. Level Master: " + DoggyServer_main.avaDatas[master].fullname);
                    foreach (UUID master in Master.secondLevelMaster)
                        client.Self.InstantMessage(fromAV, "2. Level Master: " + DoggyServer_main.avaDatas[master].fullname);
                    break;

                case "pos":
                    client.Self.InstantMessage(fromAV, Simulator_Sub.ParcelUpdate.simName + " -> " + Simulator_Sub.ParcelUpdate.parcelName + " -> " + client.Self.SimPosition.ToString());
                    break;

                case "musicurl":
                case "musikurl":
                    client.Self.InstantMessage(fromAV, Simulator_Sub.ParcelUpdate.musikURL);
                    break;

                case "inventory":
                    client.Self.InstantMessage(fromAV, "Show Invetory to Logscreen and File");
                    Inventory_Sub.Dump.all(client);
                    break;

                case "appearance":
                    foreach (UUID ava in Appearance_Sub.Data.Appearances.Keys)
                        client.Self.InstantMessage(fromAV, "Apperances for: " + DoggyServer_main.avaDatas[ava].fullname);
                    break;

                case "bots":
                    lock (client.Network.Simulators)
                    {
                        for (int i = 0; i < client.Network.Simulators.Count; i++)
                        {
                            client.Network.Simulators[i].ObjectsAvatars.ForEach(
                                delegate(Avatar av)
                                {
                                    lock (Avatars_Sub.View.m_AgentList)
                                    {
                                        if (!Avatars_Sub.View.m_AgentList.ContainsKey(av.ID))
                                        {
                                            client.Self.InstantMessage(fromAV, "Bot : " + av.Name);
                                        }
                                    }
                                }
                            );
                        }
                        break;
                     }
                    
                case "outfit":
                    Dictionary<WearableType, AppearanceManager.WearableData> wearabel = client.Appearance.GetWearables();
                    foreach (AppearanceManager.WearableData item in wearabel.Values)
                    {
                        client.Self.InstantMessage(fromAV, "Item : " + item.WearableType.ToString() + " -> " + item.ItemID.ToString());
                    }
                    break;

                case "outfits":
                    foreach (string outfit in Inventory_Sub.Data.outfits.Keys)
                    {
                        client.Self.InstantMessage(fromAV, "Outfit : " + outfit);
                    }
                    break;

                case "ao":
                    foreach (string outfit in Inventory_Sub.Data.AO.Keys)
                    {
                        client.Self.InstantMessage(fromAV, "Outfit : " + outfit);
                    }
                    break;

                case "norm":
                    foreach (InventoryBase item in Inventory_Sub.NormAppearance.normOutfit)
                    {
                        client.Self.InstantMessage(fromAV, "Item : " + item.Name);
                    }
                    break;

                case "anis":
                    foreach (UUID avaKey in Animation_Sub.Rip.anis.Keys)
                    {
                        client.Self.InstantMessage(fromAV, "Ani from : " + DoggyServer_main.avaDatas[avaKey].fullname);
                    }
                    break;

                case "groups":
                    Group_Sub.Show.all(client, fromAV);
                    client.Self.InstantMessage(fromAV, "That's all Groups");
                    break;

                case "friends":
                    Friends_Sub.Show.all(client, fromAV);
                    break;

                case "mute":
                    foreach (string avaName in DoggyServer_main.muteListByName)
                        client.Self.InstantMessage(fromAV, avaName);
                    break;

                case "evil":
                    client.Self.InstantMessage(fromAV, "Scanning for Evils");
                    foreach (UUID uuid in DoggyServer_main.avaDatas.Keys)
                        if (DoggyServer_main.avaDatas[uuid].masterLevel == 666) client.Self.InstantMessage(fromAV, DoggyServer_main.avaDatas[uuid].fullname);
                    client.Self.InstantMessage(fromAV, "finnished");
                    break;

                default:
                    client.Self.InstantMessage(fromAV, "No show command found");
                    break;
            }
        }
    }
}
