using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub
{
    class Move
    {
        private static Vector3 lastPos = new Vector3();
        private static DateTime lastPosTime = DateTime.Now;
        public static string moveString = "Stand";

        public static void to(GridClient client, Vector3 pos)
        {
            if (Vector3.Distance(pos, client.Self.SimPosition) > 10)
                client.Self.Movement.AlwaysRun = true;
            else client.Self.Movement.AlwaysRun = false;

            if (client.Self.SimPosition != lastPos)
            {
                lastPos = client.Self.SimPosition;
                lastPosTime = DateTime.Now;
            }
            else
            {
                if (Vector3.Distance(pos, client.Self.SimPosition) > 10)
                {
                    if ((DateTime.Now - lastPosTime).TotalSeconds > 40)
                    {
                        client.Self.AutoPilotLocal(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), pos.Z);
                    }
                }
            }

            Animation_Sub.AO.ResetAutoAnimation();
            Movement_Sub.Stand.ava(client);

            client.Self.Movement.TurnToward(pos);
            client.Self.AutoPilotLocal(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), pos.Z);
            
        }

        public static Boolean teleported = false;
        public static void toAndWait(GridClient client, Vector3 pos)
        {
            int sleepTimer = 50;
            int timer = 60;
            DateTime beginn = DateTime.Now;

            Vector2 selfPos = new Vector2(0, 0);
            Vector2 destPos = new Vector2(pos.X, pos.Y);

            teleported = false;

            float last_distance = Vector2.Distance(selfPos, destPos);
            float curr_distance = Vector2.Distance(selfPos, destPos);
            int error_counter = 0;

            while ((curr_distance > 2) && ((DateTime.Now - beginn).TotalSeconds < timer) && (!teleported))
            {
                to(client, pos);

                moveString = "Move Timer: " + (DateTime.Now - beginn).TotalSeconds.ToString() + " Distance: " + Vector2.Distance(new Vector2(client.Self.SimPosition.X,client.Self.SimPosition.Y), destPos).ToString();
                System.Threading.Thread.Sleep(sleepTimer);

                selfPos = new Vector2(client.Self.SimPosition.X, client.Self.SimPosition.Y);

                curr_distance = Vector2.Distance(selfPos, destPos);

                if (last_distance < curr_distance) error_counter++;
                else last_distance = curr_distance;

                if (error_counter > 3) curr_distance = 0;
            }

            beginn = DateTime.Now;
            selfPos = new Vector2(client.Self.SimPosition.X, client.Self.SimPosition.Y);
            curr_distance = Vector2.Distance(selfPos, destPos);

            if ((curr_distance > 2) && ((DateTime.Now - beginn).TotalSeconds < timer) && (!teleported))
            {
                //client.Self.

                    /*
                string lsl = "llMoveToTarget;<" + pos.X.ToString().Replace(',', '.') + "," + pos.Y.ToString().Replace(',', '.') + "," + pos.Z.ToString().Replace(',', '.') + ">;0.5";
                Output_sub.Logs.add(lsl,false);
                client.Self.Chat(lsl, DoggyServer_main.channel_hud, ChatType.Normal);

                System.Threading.Thread.Sleep(3000);

                client.Self.Chat("llStopMoveToTarget;", DoggyServer_main.channel_hud, ChatType.Normal);
                Output_sub.Logs.add("llStopMoveToTarget;", false);
                     */
            }

            moveString = "Stand";

            Movement_Sub.Stand.ava(client);
        }
    }
}
