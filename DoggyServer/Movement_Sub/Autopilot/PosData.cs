using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub.Autopilot
{

    class PosData
    {
        public static List<WayPoints> liste = new List<WayPoints>();
        private static Boolean debug = false;

        public static void init()
        {

            if (debug)
            {
                wp_add(new Vector3(172, 121, 62), false, false);
                wp_add(new Vector3(161, 121, 62), false, false);
                wp_add(new Vector3(161, 112, 62), false, false);
                wp_add(new Vector3(171, 110, 62), false, false);
                wp_add(new Vector3(177, 116, 62), false, false);
                wp_add(new Vector3(188, 116, 62), false, true);

                wp_add(new Vector3(193, 116, 62), false, false);
                wp_add(new Vector3(184, 116, 62), false, true);

                wp_add(new Vector3(177, 115, 62), false, false);
            }
            else
            {

                wp_add(new Vector3(172, 121, 62), true, false);
                wp_add(new Vector3(161, 121, 62), true, false);
                wp_add(new Vector3(161, 112, 62), true, false);
                wp_add(new Vector3(171, 110, 62), true, false);
                wp_add(new Vector3(177, 116, 62), true, false);
                //wp_add(new Vector3(188, 116, 62), true, true);

                //wp_add(new Vector3(193, 116, 62), true, false);
                //wp_add(new Vector3(184, 116, 62), true, true);

                //wp_add(new Vector3(177, 115, 62), true, false);
            }

        }

        private static void wp_add(Vector3 pos, Boolean wait, Boolean touch)
        {
            WayPoints point = new WayPoints();
            point.pos = pos;
            point.wait = wait;
            point.touch = touch;
            liste.Add(point);
        }
    }

    class WayPoints
    {
        public Vector3 pos;
        public Boolean wait = false;
        public Boolean touch = false;
    }
}
