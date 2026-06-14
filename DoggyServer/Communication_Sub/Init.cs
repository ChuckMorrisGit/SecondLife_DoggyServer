using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Init
    {
        public static void all(GridClient client)
        {
            try
            {


                AIMLbot_sub.Alice.init();

                Communication_Sub.Greeter.init();

                Communication_Sub.IM.init(client);
                Communication_Sub.Chat.init(client);
            }

            catch (Exception ex)
            {

                Output_sub.Logs.add("Communication_Sub" + ex.Message, false);

            }
        }
    }

    class close
    {
        public static void all(GridClient client)
        {
            AIMLbot_sub.Alice.close();

            Communication_Sub.Greeter.close();

            Communication_Sub.IM.close(client);
            Communication_Sub.Chat.close();

        }
    }

}
