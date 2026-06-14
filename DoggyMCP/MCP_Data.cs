using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyMCP
{
    [Serializable()]
    public class MCP_Data
    {
        public enum GetCommand
        {
            none = 0,
            relog = 1,
            say_chat = 2,
            say_im = 3,
            //say_g_im = 4,
            update_sl_client = 5,
            unknown = 6,
        }

        public enum SetCommand
        {
            none = 0,
            restart = 1,
        }

        public GetCommand get_command = GetCommand.none;
        public SetCommand set_command = SetCommand.none;
    }

}
