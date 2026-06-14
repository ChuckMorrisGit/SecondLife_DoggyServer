using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer
{
    public class DoggyServer_Multi
    {
        public static GridClient init(AvaData agentData)
        {
            GridClient client = new GridClient();

            client = LoginManager_Sub.Login.doggy(agentData);

            return (client);
        }
    }
}
