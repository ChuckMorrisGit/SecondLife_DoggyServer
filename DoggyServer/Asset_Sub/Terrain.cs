using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenMetaverse;
using System.Threading;

namespace DoggyServer.Asset_Sub
{
    class Terrain
    {
        private static AutoResetEvent xferTimeout = new AutoResetEvent(false);
        private static string fileName;

        public static void init(GridClient client)
        {
        }

        public static void close(GridClient client)
        {
        }

        public static void get(GridClient client)
        {
            int timeout = 120000; // default the timeout to 2 minutes
            fileName = client.Network.CurrentSim.Name + ".raw";


            // Create a delegate which will be fired when the simulator receives our download request
            // Starts the actual transfer request
            EventHandler<InitiateDownloadEventArgs> initiateDownloadDelegate =
                delegate(object sender, InitiateDownloadEventArgs e)
                {
                    client.Assets.RequestAssetXfer(e.SimFileName, false, false, UUID.Zero, AssetType.Unknown, false);
                };

            // Subscribe to the event that will tell us the status of the download
            client.Assets.XferReceived += new EventHandler<XferReceivedEventArgs>(Assets_XferReceived);
            // subscribe to the event which tells us when the simulator has received our request
            client.Assets.InitiateDownload += initiateDownloadDelegate;

            // configure request to tell the simulator to send us the file
            List<string> parameters = new List<string>();
            parameters.Add("download filename");
            parameters.Add(fileName);
            // send the request
            client.Estate.EstateOwnerMessage("terrain", parameters);

            // wait for (timeout) seconds for the request to complete (defaults 2 minutes)
            if (!xferTimeout.WaitOne(timeout, false))
            {
            Output_sub.Logs.add("Timeout while waiting for terrain data", false);
            }

            // unsubscribe from events
            client.Assets.InitiateDownload -= initiateDownloadDelegate;
            client.Assets.XferReceived -= new EventHandler<XferReceivedEventArgs>(Assets_XferReceived);

            // return the result

        }

        /// <summary>
        /// Handle the reply to the OnXferReceived event
        /// </summary>
        private static void Assets_XferReceived(object sender, XferReceivedEventArgs e)
        {
            if (e.Xfer.Success)
            {
                // set the result message

                // write the file to disk
                FileStream stream = new FileStream(fileName, FileMode.Create);
                BinaryWriter w = new BinaryWriter(stream);
                w.Write(e.Xfer.AssetData);
                w.Close();

                // tell the application we've gotten the file
                xferTimeout.Set();
            }
        }
    }
}

