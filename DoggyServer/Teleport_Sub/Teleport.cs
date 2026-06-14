using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Teleport_Sub
{
    class Teleport
    {
        private static GridClient client;
        private static string sim;
        private static Vector3 pos;
        private static Boolean finished = false;
        public static Boolean teleporting = false;
        public static Boolean download_terrain = false;

        public static void init(GridClient client_new)
        {
            client = client_new;
            client.Self.TeleportProgress += new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
        }
        
        public static void toNameAndWait(GridClient client, string sim_new, Vector3 pos_new)
        {
            sim = sim_new;
            pos = pos_new;
            finished = false;
            client.Self.Teleport(sim, pos);
            while (!finished) System.Threading.Thread.Sleep(500);
        }

        private static int teleportTryCounter = 100;
        private static Boolean teleportFinished = false;



        public static Boolean toSim(GridClient client, string simname, Vector3 pos)
        {
            teleporting = true;
            teleportFinished = false;

            teleportTryCounter = 1000;
            do
            {
                client.Self.Teleport(simname, pos);

                int secCounter = 60;
                while ((secCounter > 0) && (!teleportFinished))
                {
                    System.Threading.Thread.Sleep(1000);
                    secCounter--;
                }
                teleportTryCounter--;
            } while ((teleportTryCounter > 0) && (!teleportFinished));

            Movement_Sub.Autopilot.Pilot.teleported_home = false;

            teleporting = false;
            return (teleportFinished);
        }

        public static Boolean toLM_UUID(GridClient client, UUID lm)
        {
            teleporting = true;
            teleportFinished = false;

            teleportTryCounter = 1000;
            do
            {
                client.Self.Teleport(lm);

                int secCounter = 60;
                while ((secCounter > 0) && (!teleportFinished))
                {
                    System.Threading.Thread.Sleep(1000);
                    secCounter--;
                }
                teleportTryCounter--;
            } while ((teleportTryCounter > 0) && (!teleportFinished));

            if (lm == UUID.Zero) Movement_Sub.Autopilot.Pilot.teleported_home = true;
            else Movement_Sub.Autopilot.Pilot.teleported_home = false;
            teleporting = false;
            return (teleportFinished);
        }

        static void Self_TeleportProgress(object sender, TeleportEventArgs e)
        {
            Output_sub.Logs.add("TELEPORT: " + e.Message, false);

            if ((e.Status == TeleportStatus.Finished) || (e.Message == "Could not teleport closer to destination"))
            {
                Movement_Sub.Move.teleported = true;
                teleportFinished = true;
                finished = true;

                ObjectManager_Sub.Data.localPrimsNames = new Dictionary<uint, string>();

                int duration = 100;

                int start = Environment.TickCount;

                client.Self.Movement.LeftPos = true;

                Movement_Sub.Follow.deleteAutoPilotPosList();

                while (Environment.TickCount - start < duration)
                {
                    // The movement timer will do this automatically, but we do it here as an example
                    // and to make sure updates are being sent out fast enough
                    client.Self.Movement.SendUpdate(false);
                    System.Threading.Thread.Sleep(100);
                }

                client.Self.Movement.LeftPos = false;

                if (Vector3.Distance(client.Self.SimPosition, pos) > 5)
                {
                }

                if (Simulator_Sub.Parcel.CheckIfOwn(client)) Movement_Sub.Autopilot.Pilot.TimerTask();

                ObjectManager_Sub.ObjectUpdate.clearData();

                //if (download_terrain) Asset_Sub.Terrain.get(client);

                
            }
        }
    }
}
