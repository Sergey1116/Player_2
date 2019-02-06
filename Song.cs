using System;

namespace MusicPlayer
{
    public class Song : IComparable
    {
        public bool Playing;
        public int Duration;
        public string Name;
        public string Path;
        public Artist Artist;
        public Album Album;

        public int CompareTo(object obj)
        {
            return this.Name?.CompareTo((obj as Song)?.Name) ?? 0;
        }
    }
}