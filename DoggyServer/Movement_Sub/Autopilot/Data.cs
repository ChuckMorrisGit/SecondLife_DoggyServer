using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub.Autopilot
{
    class Data
    {
        public static List<MoveData> quadrate = new List<MoveData>();

        private static void add(int x1, int y1, int x2, int y2)
        {
            MoveData data = new MoveData();
            data.x1 = x1;
            data.y1 = y1;
            data.x2 = x2;
            data.y2 = y2;
            quadrate.Add(data);
        }
        public static void init(GridClient client)
        {
            quadrate = new List<MoveData>();

            add(1, 1, 254, 254);
        }
    }
}
