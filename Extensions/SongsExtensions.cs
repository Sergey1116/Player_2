using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Extensions
{
    static class  SongsExtensions
    {
        static public List<Song> Shuffle(this List<Song> songs)
        {
            Random rnd = new Random();

            for (int i = songs.Count - 1; i >= 0; i--)
            {
                var song = songs[rnd.Next(songs.Count - 1)];
                songs.Remove(song);
                songs.Add(song);
            }
            return songs;
        }
    }
}
