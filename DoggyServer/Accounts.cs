using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer
{
    class Accounts
    {
        public string firstname = string.Empty;
        public string lastname = string.Empty;
        public string fullname = string.Empty;
        public string password = string.Empty;
        public string art = string.Empty;
        public int id = 0;
        public UUID uuid = UUID.Zero;
    }
}
