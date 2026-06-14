using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenMetaverse;
using System.Diagnostics;

namespace DoggyServer
{
    class ProgramTimer
    {
        private static PerformanceCounter cpuCounter;
        private static PerformanceCounter ramCounter;

        public static string cpu_output = "CPU Usage: ";

        private static GridClient client;
        private static Timer aoTimer;
        public static void init(GridClient client_new)
        {
            client = client_new;

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);

            ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);

            aoTimer = new Timer(new TimerCallback(TimerProc), null, 0, 1000);
        }

        public static void close()
        {
            aoTimer.Dispose();
        }

        private static int secCounter = 10;
        private static int hourCounter = 0;
        private static int last_counter = 0;
        private static int restart_counter = 0;
        private static void TimerProc(object state)
        {
            Animation_Sub.AO.TimerTask();

            if (ObjectManager_Sub.ObjectUpdate.followOn) Movement_Sub.Follow.followNext(client);
            else Movement_Sub.Autopilot.Pilot.TimerTask();

            Grid_Sub.FindMaster.TimerTask();

            secCounter--;
            if (secCounter <= 0)
            {
                secCounter = 10;
                ViewerLogFile_Sub.init.TimerTask();
            }

            hourCounter--;
            if (hourCounter < 0)
            {
                hourCounter = 3600;
                //Asset_Sub.Image.checkDropbox();
                //Group_Sub.GroupMembers.sl_inside(client);
            }

            float cpu_usage = cpuCounter.NextValue();
            if (cpu_usage > 80) last_counter++;
            else
            {
                last_counter = 0;
                restart_counter = 0;
            }

            cpu_output = "CPU Usage: " + Convert.ToInt32(cpu_usage).ToString() + "% > 100: " + last_counter.ToString();

            if (last_counter > 60)
            {
                Output_sub.Logs.add("CPU USAGE COUNTER: " + last_counter.ToString(), false);
                last_counter = 0;
                Communication_Sub.MQTT.log2MQTT(cpu_output);

                restart_counter++;

                if (restart_counter > 10)
                {
                    DoggyServer_main.running = false;
                }
            }

            Output_sub.MainScreen.Output(client);

            MCP_Sub.init.ConnectStatus();
            MCP_Sub.Command.get(client);
        }
    }
}
