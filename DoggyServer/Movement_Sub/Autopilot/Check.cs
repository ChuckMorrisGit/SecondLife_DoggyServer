using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub.Autopilot
{
    class Check
    {
        public static Boolean ifAble(GridClient client)
        {
            Boolean able = false;

            foreach (MoveData data in Data.quadrate)
            {
                if (checkRechteck(data, client.Self.SimPosition)) able = true;
            }

            return (able);
        }

        private static Boolean checkRechteck(MoveData data, Vector3 pos)
        {
            Boolean inside = false;
            int xlow = data.x1;
            int xhigh = data.x2;
            int ylow = data.y1;
            int yhigh = data.y2;

            if (xlow > xhigh)
            {
                xlow = data.x2;
                xhigh = data.x1;
            }

            if (ylow > yhigh)
            {
                ylow = data.y2;
                yhigh = data.y1;
            }

            if ((pos.X >= xlow) && (pos.X <= xhigh) && (pos.Y >= ylow) && (pos.Y <= yhigh)) inside = true;
            return (inside);
        }
    }
}
