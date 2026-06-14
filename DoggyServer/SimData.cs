using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer
{
    class SimData
    {
        public int databaseID = 0;
        public string name = string.Empty;
        public int xPos = 0;
        public int yPos = 0;
        public UUID mapID = UUID.Zero;
    }
}
