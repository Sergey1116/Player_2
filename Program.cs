using System;

namespace MusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();

            player.SongStartedEvent += ShowInfo;
            player.SongsListChangedEvent += ShowInfo;
            player.VolumeEvent += ShowInfo;
            player.LockEvent += ShowInfo;
            player.SongStopEvent += ShowInfo;

            ShowInfo(player);

            while (true)
            {
                string command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        player.Play();
                        break;
                    case "2":
                        player.Stop();
                        break;
                    case "3":
                        player.LoadPlaylist(@"C:\Users\Sergey\Desktop\Test\One.xml");
                        break;
                    case "4":
                        player.Load(@"C:\Users\Sergey\Desktop\Test\WAV-Short");
                        break;
                }
            }
        }

        private static void ShowInfo(Player player)
        {
            Console.Clear();// remove old data
            //Render the list of songs
            foreach (var song in player.Songs)
            {
                if (song.Playing)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(song.Name);//Render current song in other color.
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(song.Name);
                }
            }
            //Render status bar
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Volume is: {player.Volume}. Locked: {player._isLocked}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Start = 1, Stop = 2, LoadPlaylist = 3, LoadFolder = 4.");
            Console.ResetColor();
            Console.WriteLine("Enter the number:");
        }
    }
}