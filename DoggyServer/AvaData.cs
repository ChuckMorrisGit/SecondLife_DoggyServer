using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
namespace DoggyServer
{
    public class AvaData
    {
        public enum avaType
        {
            female = 0,
            male = 1,
            doggy = 2,
            catty = 3,
            cow = 4,
            horse = 5,
        }

        public string vorname = string.Empty;
        public string nachname = string.Empty;
        public string fullname = string.Empty;
        public string password = string.Empty;
        public UUID uuid = UUID.Zero;
        public string rezDay = "unknow";
        public UUID sl_pic = UUID.Zero;
        public UUID rl_pic = UUID.Zero;
        public int masterLevel = 99;
        public Vector3 position = Vector3.Zero;
        public Boolean inSacriGroup = false;
        public Boolean online_check = false;
        public Boolean online = false;

        public avaType type = avaType.female;

        public Vector3d lookAt = Vector3d.Zero;
        public UUID lookAt_id = UUID.Zero;
        public Vector3d pointAt = Vector3d.Zero;
        public UUID pointAt_id = UUID.Zero;

        public Boolean groupInvited = false;
        public Boolean xfactorInvited = false;
    }
}
