using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Set
    {
        public static void to(GridClient client, string[] commandArray, UUID fromAV, string fromName)
        {
            try
            {
                string fullParam = string.Empty;
                int i = 2;
                while (i < commandArray.Count())
                {
                    fullParam += commandArray[i] + " ";
                    i++;
                }
                fullParam = fullParam.TrimEnd(' ');

                switch (commandArray[1])
                {
                    case "master":
                        Master.currentMaster = fromName;
                        client.Self.InstantMessage(fromAV, "Set Master to: " + Master.currentMaster);
                        break;

                    case "evil":
                        Set_Sub.Evil.ToAva(client, commandArray[2] + " " + commandArray[3], fromAV);
                        break;

                    case "home":
                        client.Self.SetHome();
                        break;

                    case "greeter":
                        if (commandArray[2].ToLower() == "on") Greeter.greeterOn = true;
                        else Greeter.greeterOn = false;
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Greeter set to: " + Greeter.greeterOn.ToString());
                        break;

                    case "chatfollow":
                        if (commandArray[2].ToLower() == "on") Chat.chat_follow = true;
                        else Chat.chat_follow = false;
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Chat Follow set to: " + Chat.chat_follow.ToString());
                        break;

                    case "metadata":
                        if (commandArray[2].ToLower() == "on") DoggyServer_main.getMetaData = true;
                        else DoggyServer_main.getMetaData = false;
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Get Metadate set to: " + Chat.chat_follow.ToString());
                        break;

                    case "auto":
                        if (commandArray[2].ToLower() == "on") Avatars_Sub.View.autoRipper=true;
                        else Avatars_Sub.View.autoRipper=false;
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "auto^^ set to: " + Avatars_Sub.View.autoRipper.ToString());
                        break;

                    case "appearance":
                        string avaName = Appearance_Sub.SetShape.byAvaName(client, fullParam, true);
                        client.Self.InstantMessage(fromAV, "Set Appearance to: " + avaName);
                        break;

                    case "outfit":
                        Inventory_Sub.NormAppearance.set(client, fromAV, commandArray[2]);
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Set norm Appearance");
                        break;

                    case "nude":
                        Appearance_Sub.Nude.all(client, fromAV);
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "I am nude now^^");
                        break;

                    case "ava":
                        if (commandArray[2].ToLower() == "on") DoggyServer_main.avaripMode = true;
                        else DoggyServer_main.avaripMode = false;
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Ava set to: " + DoggyServer_main.avaripMode.ToString());
                        break;

                    case "homeallow":
                    case "allowhome":
                        if (commandArray[2].ToLower() == "on") Simulator_Sub.Forbit.allowAllways = true;
                        else Simulator_Sub.Forbit.allowAllways = false;
                        if (fromAV != UUID.Zero) client.Self.InstantMessage(fromAV, "Allow Home set to: " + DoggyServer_main.avaripMode.ToString());
                        break;

                    case "alice_chat":
                        if (commandArray[2].ToLower() == "on") Communication_Sub.Chat.alice_bot = true;
                        else Communication_Sub.Chat.alice_bot = false;
                        break;

                    case "alice_im":
                        if (commandArray[2].ToLower() == "on") Communication_Sub.IM.alice_bot_im = true;
                        else Communication_Sub.IM.alice_bot_im = false;
                        break;
                }
            }
            catch (Exception ex) { client.Self.InstantMessage(fromAV, "More params? -> " + ex.Message); }
        }
    }
}
