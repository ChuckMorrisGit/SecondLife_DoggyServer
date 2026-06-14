using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyMCP.Net_Sub
{
    class Commands
    {
        public static Dictionary<UUID, List<MCP_Data.GetCommand>> get = new Dictionary<UUID, List<MCP_Data.GetCommand>>();
    }
}
