using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Communication_Sub
{
    class Titler
    {
        public enum which
        {
            norm = 0,
            lyric = 1,
            random = 2,
        }

        private static string[] texte =
        {
            "The Title is on the forbidden script Black list\nWe will investigate every use of it\nPlease check your Emails or SecondLife Dashboard for details",
            "I am a poor little girl",
            "Who sold me this fucking titler for 1.000L$",
            "OK. I am looking like a noob. But give me a chance",
            "What is a titler",
            "Who stole the Linden Baby?",
            "LOOSER\n*\n*\n*\n*\\/",
            "You like it Doggy in Style",
            "Do you see my 'Kick me' sign?",
            "Is that Pee in my pants?",
            "call me Quasimodo",
            "Lisa! Do I have a pants on?",
            "I need a man. A REAL men",
            "Have you ever mask a NMI?",
            "*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\nPlease follow the stars",
            "If you can read this, you are to close^^",
            "Beware of my smelling finger",
            "RUN!!!!! OMG... RUN!!!!!!\nI am exploding",
            "Do you think my ass is too fat?",
            "If you have a question or I can help you just send me an IM\nanytime\nIf you want something more, I can give you my phone number^^",
            "Please speak clear and loud. I am stupid",
            "1 2 3 4 6 7 ... shit. ok ok. Please give me another chance",
            "."
        };

        public static which whichOutput = which.random;
        private static Random rand = new Random();

        public static void response(GridClient client, string msg)
        {
            string[] msg1 = msg.Split('/');

            string[] msg2 = msg1[1].Split(' ');

            int channel = int.Parse(msg2[0]);


            if (channel != 0)
            {
                switch (whichOutput)
                {
                    case which.norm:
                        client.Self.Chat("The Title is on the forbidden script Black list\nWe will investigate every use of it\nPlease check your Emails for details", channel, ChatType.Shout);
                        break;

                    case which.lyric:
                        client.Self.Chat(Media_sub.Daten.lyric, channel, ChatType.Shout);
                        break;

                    case which.random:
                        float ran = rand.Next(texte.Count());
                        client.Self.Chat(texte[(int)Math.Floor(ran)], channel, ChatType.Shout);
                        break;

                }
            }
        }
    }
}
