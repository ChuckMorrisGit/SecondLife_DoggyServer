using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
namespace DoggyServer.Movement_Sub.Target
{
    class Data
    {
        public static Dictionary<string, List<Vector3>> waypoints = new Dictionary<string, List<Vector3>>();
        public static Dictionary<string, UUID> teleKey = new Dictionary<string, UUID>();
    }
}
