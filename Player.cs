using System;
using System.Collections.Generic;
using System.IO;

namespace MusicPlayer
{
    public class Player
    {
        const int MIN_VOLUME = 0;
        const int MAX_VOLUME = 100;

        public bool _isLocked { get; private set; }


        private bool _isPlaying;

        private int _volume;
        public int Volume
        {
            get
            {
                return _volume;
            }

            private set
            {
                if (value < MIN_VOLUME)
                {
                    _volume = MIN_VOLUME;
                }
                else if (value > MAX_VOLUME)
                {
                    _volume = MAX_VOLUME;
                }
                else
                {
                    _volume = value;
                }
            }
        }

        public List<Song> Songs { get; } = new List<Song>();
        //public Song PlayingSong { get; private set; }

        public event Action<Player> SongsListChangedEvent;
        public event Action<Player> SongStartedEvent;
        public event Action<Player> VolumeEvent;
        public event Action<Player> LockEvent;


        public void VolumeUp()
        {
            if (_isLocked == false)
            {
                Volume++;
                VolumeEvent?.Invoke(this);
            }
        }

        public void VolumeDown()
        {
            if (_isLocked == false)
            {
                Volume--;
                VolumeEvent?.Invoke(this);
            }
        }

        public void VolumeChange(int step)
        {
            if (_isLocked == false)
            {
                Volume += step;
                VolumeEvent?.Invoke(this);
            }
        }

        public void Load(string source)
        {
            var dirInfo = new DirectoryInfo(source);

            if (dirInfo.Exists)
            {
                var files = dirInfo.GetFiles();
                foreach (var file in files)
                {
                    var song = new Song
                    {
                        Path = file.FullName,
                        Name = file.Name
                    };
                    Songs.Add(song);
                }
            }

            SongsListChangedEvent?.Invoke(this);
        }

        public void Play()
        {
            if (!_isLocked && Songs.Count > 0)
            {
                _isPlaying = true;
            }

            if (_isPlaying)
            {
                foreach (var song in Songs)
                {
                    song.Playing = true;
                    SongStartedEvent?.Invoke(this);

                    using (System.Media.SoundPlayer player = new System.Media.SoundPlayer())
                    {
                        player.SoundLocation = song.Path;
                        player.PlaySync();
                    }
                    song.Playing = false;
                }
            }

            _isPlaying = false;
        }

        public bool Stop()
        {
            if (!_isLocked)
            {
                _isPlaying = false;
            }

            return _isPlaying;
        }

        public void Clear()
        {
            Songs.Clear();
        }

        public void Lock()
        {
            _isLocked = true;
            LockEvent?.Invoke(this);
        }

        public void Unlock()
        {
            _isLocked = false;
            LockEvent?.Invoke(this);
        }
    }
}