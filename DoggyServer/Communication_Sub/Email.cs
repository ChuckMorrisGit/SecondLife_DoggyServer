using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DoggyServer.Communication_Sub
{
    class Email
    {
        private static string send_to = "sensor@example.com";
        private static string fromAdress = SecretsManager.SmtpFromAddress;
        private static string password = SecretsManager.SmtpPassword;

        public static Boolean send(string subject, string body)
        {
            Boolean is_sended = false;


            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAdress); //Absender 
            mail.To.Add(send_to); //Empfänger 
            mail.Subject = subject;
            mail.Body = body;
            //mail.IsBodyHtml = true; //Nur wenn Body HTML Quellcode ist  

            SmtpClient client = new SmtpClient(SecretsManager.SmtpServer, SecretsManager.SmtpPort); //SMTP Server 
                                                                       //SmtpClient client = new SmtpClient("smtp.live.com", 587); //SMTP Server von Hotmail und Outlook. 

            int tries = 3;
            while (tries > 0)
            {
                try
                {
                    client.Credentials = new System.Net.NetworkCredential(fromAdress, password);//Anmeldedaten für den SMTP Server 

                    client.EnableSsl = true; //Die meisten Anbieter verlangen eine SSL-Verschlüsselung 

                    ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };


                    client.Send(mail); //Senden 

                    tries = 0;
                    is_sended = true;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(30000);
                    tries--;

                }
            }

            return is_sended;
        }
    }
}

