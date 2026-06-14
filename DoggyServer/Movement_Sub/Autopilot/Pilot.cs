using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Movement_Sub.Autopilot
{
    class Pilot
    {
        private static GridClient client;
        private static Random rand = new Random();
        private static int wegPointCounter = 0;
        public static Boolean teleported_home = false;
        

        public static void init(GridClient client_new)
        {
            client = client_new;
        }

        public static void TimerTask()
        {
            if ((!Teleport_Sub.Teleport.teleporting) && (teleported_home))
            {
                if ((Animation_Sub.AO.nothingToDo) && (!ObjectManager_Sub.ObjectUpdate.followOn))
                {
                    switch (DoggyServer_main.agentData.type)
                    {
                        case AvaData.avaType.doggy:
                        case AvaData.avaType.catty:
                            if ((Simulator_Sub.Parcel.CheckIfOwn(client)) && (Check.ifAble(client)))
                            {
                                Boolean wait = false;
                                do
                                {
                                    Vector3 newPos = PosData.liste[wegPointCounter].pos;
                                    wait = PosData.liste[wegPointCounter].wait;
                                    Boolean touch = PosData.liste[wegPointCounter].touch;

                                    wegPointCounter++;
                                    if (wegPointCounter >= PosData.liste.Count) wegPointCounter = 0;

                                    Output_sub.Logs.add("Goto: " + newPos.X.ToString() + " - " + newPos.Y.ToString(), false);
                                    Movement_Sub.Move.toAndWait(client, newPos);
                                    if (!wait)
                                    {
                                        Output_sub.Logs.add("Pilot Wait 3 sek", false);
                                        System.Threading.Thread.Sleep(3000);
                                    }

                                    if (touch)
                                    {
                                        Output_sub.Logs.add("TRY TOUCH", false);
                                        List<Primitive> prims = ObjectManager_Sub.FindObejct.inRange(client, 64);
                                        Output_sub.Logs.add("FOUND " + prims.Count.ToString() + " PRIMS", false);

                                        int counter = 0;

                                        while (counter < prims.Count)
                                        {
                                            try
                                            {
                                                Primitive prim = prims[counter];

                                            //Output_sub.Logs.add(prim.LocalID.ToString() + " Parent: " + prim.ParentID.ToString() + ": PRIM FLAGS: " + counter.ToString() + " -> " + prim.Flags.ToString(), false);
                                            //System.Threading.Thread.Sleep(100);

                                                if (ObjectManager_Sub.ObjectUpdate.prim_properties.Keys.Contains(prim.LocalID))
                                                    prim.Properties = ObjectManager_Sub.ObjectUpdate.prim_properties[prim.LocalID].Properties;
                                                else
                                                    prim.Properties = ObjectManager_Sub.ObjectUpdate.getProperty(client, prim.LocalID, false).Properties;

                                                

                                                if (Vector3.Distance(prim.Position, client.Self.SimPosition) > -1)
                                                {


                                                    //Output_sub.Logs.add(prim.LocalID.ToString() + ": " + name, false);

                                                    if ((prim.Flags & PrimFlags.Touch) != 0)
                                                    {
                                                        if (prim.Properties.Name.ToLower().Contains("door"))
                                                        {
                                                            if (Simulator_Sub.ParcelUpdate.parcelName == "Master Residenz")
                                                            {
                                                                Output_sub.Logs.add("TOUCH: " + prim.LocalID.ToString() + " Parent: " + prim.ParentID.ToString(), false);
                                                                client.Self.Touch(prim.LocalID);

                                                                Movement_Sub.Goto.forward(client, UUID.Zero, (float)2.0);

                                                                System.Threading.Thread.Sleep(1000);
                                                                client.Self.Touch(prim.LocalID);
                                                            }
                                                        }
                                                        //System.Threading.Thread.Sleep(100);

                                                    }
                                                }
                                            }
                                            catch (Exception ex) { Output_sub.Logs.add("TOUCH ERROR: " + ex.Message, false); }

                                            counter++;
                                        }
                                    }
                                }
                                while (!wait);
                            }
                            break;
                    }
                }
            }
        }
    }
}

