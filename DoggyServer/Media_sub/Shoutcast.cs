using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using OpenMetaverse;
using System.Text.RegularExpressions;

namespace DoggyServer.Media_sub
{
    class Shoutcast
    {
        //[STAThread]
        public static void info()
        {
            // examplestream: Radio ABF - http://www.radioabf.net

            // url: http://relay.pandora.radioabf.net:9000


            // station parameters

            String server = "http://relay.pandora.radioabf.net:9000";
            String serverPath = "/";

            String destPath = "C:\\"; // destination path for saved songs

            HttpWebRequest request = null; // web request
            HttpWebResponse response = null; // web response

            int metaInt = 0; // blocksize of mp3 data
            int count = 0; // byte counter
            int metadataLength = 0; // length of metadata header

            // metadata header that contains the actual songtitle
            string metadataHeader = "";
            // last metadata header, to compare with 
            // new header and find next song
            string oldMetadataHeader = null;

            byte[] buffer = new byte[512]; // receive buffer

            // input stream on the webrequest
            Stream socketStream = null;
            // output stream on the destination file
            Stream byteOut = null;

            // create request
            request = (HttpWebRequest)WebRequest.Create(server);

            // clear old request header and build 
            // own header to activate Icy-metadata
            request.Headers.Clear();
            request.Headers.Add("GET", serverPath + " HTTP/1.0");
            // needed to receive metadata informations
            request.Headers.Add("Icy-MetaData", "1");
            request.UserAgent = "WinampMPEG/5.09";

            // execute request
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            // read blocksize to find metadata block
            metaInt = Convert.ToInt32(response.GetResponseHeader("icy-metaint"));

            try
            {
                // open stream on response

                socketStream = response.GetResponseStream();

                // rip stream in an endless loop

                while (true)
                {
                    // read byteblock

                    int bytes = socketStream.Read(buffer,
                                             0, buffer.Length);
                    if (bytes < 0)
                        return;

                    for (int i = 0; i < bytes; i++)
                    {
                        // if there is a header, the 'metadataLength' 

                        // would be set to a value != 0. Then we save 

                        // the header to a string

                        if (metadataLength != 0)
                        {
                            metadataHeader += Convert.ToChar(buffer[i]);
                            metadataLength--;
                            // all metadata informations were written 

                            // to the 'metadataHeader' string

                            if (metadataLength == 0)
                            {
                                string fileName = "";

                                // if songtitle changes, create a new file

                                if (!metadataHeader.Equals(oldMetadataHeader))
                                {
                                    // flush and close old byteOut stream

                                    if (byteOut != null)
                                    {
                                        byteOut.Flush();
                                        byteOut.Close();
                                    }

                                    // extract songtitle from metadata header. 

                                    // Trim was needed, because some stations 

                                    // don't trim the songtitle

                                    fileName = Regex.Match(metadataHeader,
                                        "(StreamTitle=')(.*)(';StreamUrl)").Groups[2].Value.Trim();

                                    // write new songtitle to console for information

                                    Console.WriteLine(fileName);

                                    // create new file with the songtitle from 

                                    // header and set a stream on this file

                                   //byteOut = createNewFile(destPath, fileName);

                                    // save new header to 'oldMetadataHeader' string, 

                                    // to compare if there's a new song starting

                                    oldMetadataHeader = metadataHeader;
                                }
                                metadataHeader = "";
                            }
                        }
                        // write mp3 data to file or extract metadata headerlength

                        else
                        {
                            if (count++ < metaInt) // write bytes to filestream
                            {
                                // as long as we don't have a songtitle, 

                                // we don't open a new file and don't write any bytes

                                if (byteOut != null)
                                {
                                    byteOut.Write(buffer, i, 1);
                                    if (count % 100 == 0)
                                        byteOut.Flush();
                                }
                            }
                            // get headerlength from lengthbyte and 

                            // multiply by 16 to get correct headerlength

                            else
                            {
                                metadataLength = Convert.ToInt32(buffer[i]) * 16;
                                count = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (byteOut != null)
                    byteOut.Close();
                if (socketStream != null)
                    socketStream.Close();
            }
        }
    }
}
