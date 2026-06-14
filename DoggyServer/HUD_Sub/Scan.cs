using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.HUD_Sub
{
    class Scan
    {
        public static uint hudRootPrim(GridClient client, string hudName,UUID fromAva)
        {
            uint rootPrimLocalID = 0;
            List<Primitive> attachments = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive prim) { return prim.ParentID == client.Self.LocalID; }
            );

            for (int i = 0; i < attachments.Count; i++)
            {
                Primitive prim = attachments[i];
                AttachmentPoint point = StateToAttachmentPoint(prim.PrimData.State);

                Primitive primProperty = ObjectManager_Sub.ObjectUpdate.getProperty(client, prim.LocalID,true);
                if (primProperty != null)
                {
                    prim.Properties = primProperty.Properties;
                    if (prim.Properties.Name == hudName)
                    {
                        rootPrimLocalID=prim.LocalID;
                        client.Self.InstantMessage(fromAva, prim.Properties.Name);
                    }
                }
            }

            return(rootPrimLocalID);
        }

        public static List<Primitive> childPrimList(GridClient client, uint rootPrimLocalID, UUID fromAva)
        {
            List<Primitive> hud = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive hudprim) { return hudprim.ParentID == rootPrimLocalID; }
            );

            List<Primitive> prims = new List<Primitive>();
            for (int i2 = 0; i2 < hud.Count; i2++)
            {
                Primitive primProperty = ObjectManager_Sub.ObjectUpdate.getProperty(client, hud[i2].LocalID,true);
                if (primProperty != null)
                {
                    hud[i2].Properties = primProperty.Properties;
                    prims.Add(hud[i2]);
                    client.Self.InstantMessage(fromAva, primProperty.Properties.Name);
                }
            }

            return (prims);
        }

        private static AttachmentPoint StateToAttachmentPoint(uint state)
        {
            const uint ATTACHMENT_MASK = 0xF0;
            uint fixedState = (((byte)state & ATTACHMENT_MASK) >> 4) | (((byte)state & ~ATTACHMENT_MASK) << 4);
            return (AttachmentPoint)fixedState;
        }
    }
}
