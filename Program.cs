using System;
using System.Collections.Generic;

namespace MusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();
            player.Load(@"C:\Users\Sergey\Desktop\Test\WAV-Short");

            player.SongStartedEvent += ShowInfo;
            player.SongsListChangedEvent += ShowInfo;
            player.VolumeEvent += ShowInfo;
            player.LockEvent += ShowInfo;

            player.Play();
            player.VolumeChange(50);

            player.Play();
            player.Lock();

            player.Play();
            player.Unlock();

            Console.ReadLine();
        }

        private static void ShowInfo(Player player)
        {
            Console.Clear();// remove old data

            //Render the list of songs
            foreach (var song in player.Songs)
            {
                if (song.Playing == true)
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
            Console.ResetColor();
        }
    }
}