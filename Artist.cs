namespace MusicPlayer
{
    public class Artist
    {
        public string Genre;
        public string Name;

        public Artist(): this("Default artist")
        {
        }

        public Artist(string name): this(name, "Default genre")
        {
        }

        public Artist(string name, string genre)
        {
            this.Name = name;
            this.Genre = genre;
        }
    }
}
