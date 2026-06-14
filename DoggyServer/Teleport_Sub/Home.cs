using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Teleport_Sub
{
    class Home
    {
        private static UUID pindownKey = new UUID("952d5014-94dd-d776-6e57-0116d4a78b21");
        public static Boolean tpNormalHome = false;

        public static void go(GridClient client)
        {
            ObjectManager_Sub.ObjectUpdate.followOn = false;
            Animation_Sub.AO.ResetAutoAnimation();
            Movement_Sub.Stand.ava(client);

            UUID teleportKey = UUID.Zero;


            switch (LoginManager_Sub.Login.loginURI)
            {
                case LoginManager_Sub.Grids.sl_beta:
                    Teleport.toSim(client, "Bonifacio",new Vector3(125, 10, 45 ));
                    //System.Threading.Thread.Sleep(3000);
                    //Movement_Sub.Move.toAndWait(client, new Vector3(125, 7, 21));
                    break;

                default:
                    if (!tpNormalHome) teleportKey = pindownKey;
                    Teleport.toLM_UUID(client, teleportKey);
                    break;
            }

        }
    }
}