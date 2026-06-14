using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Animation_Sub
{
    class Typing
    {
        private static UUID typingAnimationID = new UUID("c541c47f-e0c0-058b-ad1a-d6ae3a4584d9");

        public static void set(GridClient client, bool typing)
        {
            if (!client.Network.Connected) return;

            Dictionary<UUID, bool> typingAnim = new Dictionary<UUID, bool>();
            typingAnim.Add(typingAnimationID, typing);

            client.Self.Animate(typingAnim, false);

            if (typing)
                client.Self.Chat(string.Empty, 0, ChatType.StartTyping);
            else
                client.Self.Chat(string.Empty, 0, ChatType.StopTyping);
        }
    }
}
