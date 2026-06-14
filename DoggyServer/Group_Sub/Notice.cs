using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Group_Sub
{
    class Notice
    {
        public static void init(GridClient client)
        {

        }

        public static void sendUUID(GridClient client, UUID groupID,string subject, string message)
        {
            GroupNotice notice = new GroupNotice();

            notice.Message = message;
            notice.Subject = subject;
            
            client.Groups.SendGroupNotice(groupID, notice);

        }
    }
}
