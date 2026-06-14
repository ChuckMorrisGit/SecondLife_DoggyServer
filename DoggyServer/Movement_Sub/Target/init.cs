using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Movement_Sub.Target
{
    class init
    {
        public static void all (GridClient client)
        {
            string target = "shop:old london docks";

            Data.teleKey.Add(target, new UUID("8fec73a7-27f1-dfad-295d-d73010d2776e"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(81, 77, 22));
            Data.waypoints[target].Add( new Vector3(36, 78, 22));
            Data.waypoints[target].Add( new Vector3(38, 157, 22));
            Data.waypoints[target].Add( new Vector3(53, 158, 22));
            Data.waypoints[target].Add( new Vector3(54, 147, 23));

            target = "shop:dark sun";

            Data.teleKey.Add(target, new UUID("dde1188f-efca-ee21-1613-36c9e54e81ef"));
            Data.waypoints.Add(target, new List<Vector3>());

            Data.waypoints[target].Add(new Vector3(104,56,68));
            Data.waypoints[target].Add(new Vector3(90, 51, 68));
            Data.waypoints[target].Add(new Vector3(89,42,68));

            target = "club:dark sun";

            Data.teleKey.Add(target, new UUID("dde1188f-efca-ee21-1613-36c9e54e81ef"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(121, 48, 83));
            Data.waypoints[target].Add(new Vector3(115, 56, 83));
            Data.waypoints[target].Add(new Vector3(103, 57, 82));
            Data.waypoints[target].Add(new Vector3(101, 70, 76));
            Data.waypoints[target].Add(new Vector3(114,68,69));


            Data.waypoints[target].Add(new Vector3(104, 56, 68));
            Data.waypoints[target].Add(new Vector3(90, 51, 68));
            Data.waypoints[target].Add(new Vector3(89, 42, 68));
            Data.waypoints[target].Add(new Vector3(89,22,68));
            Data.waypoints[target].Add(new Vector3(107,22,59));
            Data.waypoints[target].Add(new Vector3(105,15,58));
            Data.waypoints[target].Add(new Vector3(92, 17, 58));
            Data.waypoints[target].Add(new Vector3(94, 46, 58));
            Data.waypoints[target].Add(new Vector3(103,46,58));

            target = "club:x-factor";

            Data.teleKey.Add(target, new UUID("a9e3aefb-7519-68e4-1935-b7102b6b3c56"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(46,102,30));
            Data.waypoints[target].Add(new Vector3(45,118,25));
            Data.waypoints[target].Add(new Vector3(29,118, 21));
            Data.waypoints[target].Add(new Vector3(31,97,21));

            target = "misc:pinedown";
            Data.teleKey.Add(target, new UUID("952d5014-94dd-d776-6e57-0116d4a78b21"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(174, 116, 62));

            target = "misc:tierroom";
            Data.teleKey.Add(target, new UUID("8f5b04e9-be1c-d717-10d0-4abb80fe3503"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(159,163,551));

            target = "shop:virus";
            Data.teleKey.Add(target, new UUID("d9758418-5ff3-fc67-432d-3add6d73e549"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(119, 95, 1001));
            Data.waypoints[target].Add(new Vector3(76, 94, 1001));

            target = "shop:caprica";
            Data.teleKey.Add(target, new UUID("59afa837-0190-a184-dfb3-6d519b4bd4eb"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(229, 62, 2048));

            target = "own:home";
            Data.teleKey.Add(target, new UUID("d4a475de-7c98-23b8-e74c-d3b087458a92"));
            Data.waypoints.Add(target, new List<Vector3>());
            Data.waypoints[target].Add(new Vector3(89, 66, 21));
            Data.waypoints[target].Add(new Vector3(84,163,68));
        }
    }
}
