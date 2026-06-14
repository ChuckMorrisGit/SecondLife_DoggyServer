using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Movement_Sub
{
    class Follow
    {
        private static float followDistance = 1;
        private static List<AutoPilotPos> autoPilotPosList = new List<AutoPilotPos>();
        private static Vector3 lastPos = Vector3.Zero;

        public static void master(GridClient client, Vector3 pos, Boolean flying)
        {
            if (Vector3.Distance(pos, Vector3.Zero) > 5)
            {
                if (Vector3.Distance(pos, client.Self.SimPosition) > followDistance)
                {
                    if (Vector3.Distance(pos, lastPos) > 2)
                    {
                        lastPos = pos;
                        AutoPilotPos autoPos = new AutoPilotPos();
                        autoPos.x = Convert.ToInt32(pos.X);
                        autoPos.y = Convert.ToInt32(pos.Y);
                        autoPos.z = pos.Z;
                        autoPos.flying = flying;

                        autoPilotPosList.Add(autoPos);
                    }
                }
            }
        }

        public static void deleteAutoPilotPosList()
        {
            autoPilotPosList = new List<AutoPilotPos>();
        }

        private static Thread followThread;
        public static void followNext(GridClient client)
        {
            if (!followRunning)
            {
                followClient = client;
                followThread = new Thread(new ThreadStart(follow_Run));
                followThread.IsBackground = true;
                followThread.Priority = ThreadPriority.Highest;
                followThread.Start();
            }
        }

        private static GridClient followClient;
        private static Boolean followRunning = false;
        public static void follow_Run ()
        {
            followRunning = true;
            try
            {

                while (autoPilotPosList.Count > 0)
                {
                    AutoPilotPos posNew = autoPilotPosList[0];
                    autoPilotPosList.Remove(posNew);

                    followClient.Self.AutoPilotLocal(posNew.x, posNew.y, posNew.z);

                    Move.toAndWait(followClient, new Vector3((float)posNew.x, (float)posNew.y, posNew.z));

                }
            }
            catch (Exception ex) { Output_sub.Logs.add("FOLLOW: " + ex.Message, false); }
            followRunning = false;
        }
    }

    class AutoPilotPos
    {
        public Int32 x = 0;
        public Int32 y = 0;
        public float z = 0f;
        public Boolean flying = false;
    }
}

