using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Animation_Sub
{
    class AO
    {
        private static GridClient client;
        public static int tickCounter = 0;
        private static Vector3 oldPos = new Vector3();
        public static Boolean nothingToDo = false;
        private static Boolean aniPlayed = false;
        private static int standTicker = 1;
        private static int standCounter = 0;
        private static int walkCounter = 0;
        public static int standTimer = 5;
        public static int nothing2DoTimer = 40;
        public static int nothing2DoTimerAdd = 10;
        private static Random rand = new Random();

        public static void init(GridClient client_new)
        {
            client = client_new;
            //client.Self.AnimationsChanged += new EventHandler<AnimationsChangedEventArgs>(Self_AnimationsChanged);
        }

        static void Self_AnimationsChanged(object sender, AnimationsChangedEventArgs e)
        {
            e.Animations.ForEach(delegate(UUID key)
            {
                //Output_sub.Logs.add("CHANGED ANI: " + key.ToString() + " - " + e.Animations[key].ToString(), false);
            }
            );
        }

       
        public static void ResetAutoAnimation()
        {
            tickCounter = 0;
            nothingToDo = false;
        }

        private static UUID currentWalk = UUID.Zero;
        private static Vector3 walkZiel = Vector3.Zero;
        public static void TimerTask()
        {
            Vector3 pos = client.Self.SimPosition;

            if (DoggyServer_main.agentData.type == AvaData.avaType.female)
            {

                if (Vector3.Distance(pos, oldPos) > 0.2)
                {
                    if (currentWalk == UUID.Zero)
                    {
                        currentWalk = Animation_Sub.Data.walks[walkCounter];
                        client.Self.AnimationStart(currentWalk, false);

                        walkCounter++;
                        if (walkCounter >= Data.walks.Count) walkCounter = 0;
                    }
                }
                else
                {
                    if (currentWalk != UUID.Zero)
                    {
                        client.Self.AnimationStop(currentWalk, true);
                        currentWalk = UUID.Zero;
                        standTicker = 0;
                    }

                    standTicker--;
                    if (standTicker <= 0)
                    {
                        standTicker = 20;

                        client.Self.AnimationStart(Data.stands[standCounter], true);
                        standCounter++;
                        if (standCounter >= Data.stands.Count) standCounter = 0;
                    }
                }
            }

            if (pos != oldPos)
            {
                oldPos = pos;
                tickCounter = 0;
                aniPlayed = false;
                nothingToDo = false;
            }
            else
            {
                tickCounter++;

                if (tickCounter > standTimer)
                {
                    if ((client.Self.Name != "Pferd Ava") && (DoggyServer_main.agentData.type != AvaData.avaType.female) && (DoggyServer_main.agentData.type != AvaData.avaType.male))
                    {
                        if (client.Self.SittingOn == 0) client.Self.SitOnGround();
                    }
                }


                if (client.Self.SittingOn == 0)
                {
                    if (tickCounter > (nothing2DoTimer +nothing2DoTimerAdd))
                    {
                        nothingToDo = true;
                        nothing2DoTimerAdd = (int)rand.Next(40);
                    }

                    if (tickCounter > nothing2DoTimer + 100)
                    {
                        if ((DoggyServer_main.agentData.type == AvaData.avaType.doggy) && (!aniPlayed))
                        {
                            aniPlayed = true;
                            Play.doggyClean(client);
                        }
                    }

                    if (tickCounter > 7200)
                    {
                        nothingToDo = true;
                        Teleport_Sub.Home.go(client);
                    }
                }
            }
        }
    }
}
