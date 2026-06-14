using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using GetLyrics.LyricWiki_Sub;
using OpenMetaverse;

namespace DoggyServer.Media_sub
{
    class Lyrics
    {
        public static void init(GridClient client)
        {


        }

        public static void getAndSet(string artist, string title)
        {
//Daten.lyric = GetLyrics.LyricWiki_Sub.Lyric.get(artist, title);
            Output_sub.Logs.add(Daten.lyric, false);
        }
    }
}
