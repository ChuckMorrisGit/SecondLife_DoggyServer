using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Appearance_Sub
{
    class Nude
    {
        public static void all(GridClient client, UUID fromAva)
        {
            clothes(client, fromAva);
            prims(client, fromAva);
        }

        public static void clothes(GridClient client, UUID fromAva)
        {
            Dictionary<WearableType, AppearanceManager.WearableData> wear = client.Appearance.GetWearables();

            foreach (WearableType type in wear.Keys)
            {
                if (Inventory_Sub.Data.items.ContainsKey(wear[type].ItemID))
                {
                    InventoryItem item = Inventory_Sub.Data.items[wear[type].ItemID];

                    switch (type)
                    {
                        case WearableType.Undershirt:
                        case WearableType.Underpants:
                        case WearableType.Socks:
                        case WearableType.Skirt:
                        case WearableType.Shoes:
                        case WearableType.Pants:
                        case WearableType.Jacket:
                        case WearableType.Gloves:
                        case WearableType.Shirt:
                        case WearableType.Alpha:

                            client.Appearance.RemoveFromOutfit(item);
                            client.Self.InstantMessage(fromAva, "take off: " + type.ToString() + " " + item.Name);
                            break;
                    }
                }
            }
        }

        public static void prims(GridClient client, UUID fromAva)
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
                    case AttachmentPoint.Skull:
                    case AttachmentPoint.HUDBottom:
                        break;

                    case AttachmentPoint.LeftFoot:
                    case AttachmentPoint.LeftForearm:
                    case AttachmentPoint.LeftHand:
                    case AttachmentPoint.LeftHip:
                    case AttachmentPoint.LeftLowerLeg:
                    case AttachmentPoint.LeftPec:
                    case AttachmentPoint.LeftShoulder:
                    case AttachmentPoint.LeftUpperArm:
                    case AttachmentPoint.LeftUpperLeg:
                    case AttachmentPoint.Pelvis:
                    case AttachmentPoint.RightFoot:
                    case AttachmentPoint.RightForearm:
                    case AttachmentPoint.RightHand:
                    case AttachmentPoint.RightHip:
                    case AttachmentPoint.RightLowerLeg:
                    case AttachmentPoint.RightPec:
                    case AttachmentPoint.RightShoulder:
                    case AttachmentPoint.RightUpperArm:
                    case AttachmentPoint.RightUpperLeg:
                    case AttachmentPoint.Spine:
                    case AttachmentPoint.Stomach:
                        for (int i2 = 0; i2 < prim.NameValues.Length; i2++)
                        {
                            if (prim.NameValues[i2].Name == "AttachItemID")
                            {
                                UUID inventoryID = new UUID(prim.NameValues[i2].Value.ToString());
                                client.Appearance.Detach(inventoryID);
                                client.Self.InstantMessage(fromAva, "Detach from: " + point.ToString());
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
