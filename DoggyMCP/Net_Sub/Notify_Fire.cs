using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyMCP.Net_Sub
{
    class Notify_Fire
    {
        public static void to_all(SL_Client_Sub.SL_Notify sl_notify)
        {
            try
            {
                SimpleCallback.FireNewBroadcastedNotifyEvent(sl_notify);

            }
            catch (Exception ex) { Output.WriteLine("NOTIFY ERROR: " + ex.Message); }
        }
    }
}
