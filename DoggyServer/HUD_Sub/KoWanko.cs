using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.HUD_Sub
{
    class KoWanko
    {
        private static string aoName = "KoWanko AO";

        public static void get(GridClient client)
        {
            List<Primitive> attachments = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
               delegate(Primitive prim) { return prim.ParentID == client.Self.LocalID; }
           );
            
            List<InventoryItem> items = new List<InventoryItem>();
            for (int i = 0; i < attachments.Count; i++)
            {
                Primitive prim = attachments[i];
                AttachmentPoint point = StateToAttachmentPoint(prim.PrimData.State);

                switch (point)
                {
                    case AttachmentPoint.HUDBottomRight:
                        for (int i2 = 0; i2 < prim.NameValues.Length; i2++)
                        {
                            if (prim.NameValues[i2].Name == "AttachItemID")
                            {
                                UUID inventoryID = new UUID(prim.NameValues[i2].Value.ToString());
                                client.Appearance.Detach(inventoryID);
                            }
                        }
                        break;
                }
            }
        }

        private static AttachmentPoint StateToAttachmentPoint(uint state)
        {
            const uint ATTACHMENT_MASK = 0xF0;
            uint fixedState = (((byte)state & ATTACHMENT_MASK) >> 4) | (((byte)state & ~ATTACHMENT_MASK) << 4);
            return (AttachmentPoint)fixedState;
        }
    }
}
