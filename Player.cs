using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MusicPlayer
{
    public class Player
    {
        const int MIN_VOLUME = 0;
        const int MAX_VOLUME = 100;

        public bool _isLocked { get; private set; }
        public bool _isPlaying { get; private set; }

        private static CancellationTokenSource cts = new CancellationTokenSource();

        private int _volume;
        private SoundPlayer player;

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

        public List<Song> Songs { get; private set; } = new List<Song>();

        public event Action<Player> SongsListChangedEvent;
        public event Action<Player> SongStartedEvent;
        public event Action<Player> SongStopEvent;
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
            Clear();
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

        public async void Play()
        {
            if (!_isLocked && Songs.Count > 0)
            {
                _isPlaying = true;
            }

            if (_isPlaying)
            {
                cts = new CancellationTokenSource();
                foreach (var song in Songs)
                {
                    if (cts.Token.IsCancellationRequested)
                        break;
                    await Task.Run(() => {
                        song.Playing = true;
                        SongStartedEvent?.Invoke(this);

                        using (player = new SoundPlayer())
                        {
                            player.SoundLocation = song.Path;
                            player.PlaySync();
                        }
                    });
                    song.Playing = false;
                }
            }

            _isPlaying = false;
        }

        public void Stop()
        {
            if (!_isLocked)
            {
                player?.Stop();
                cts.Cancel();
                _isPlaying = false;
                SongStopEvent?.Invoke(this);
            }
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

        public void SaveAsPlaylist(string name)
        {
            XmlSerializer serializer = new XmlSerializer(Songs.GetType());
            using (FileStream fs = new FileStream($"{name}.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, Songs);
            }
        }

        public void LoadPlaylist(string name)
        {
            Clear();
            XmlSerializer serializer = new XmlSerializer(Songs.GetType());
            if (File.Exists($"{name}.xml"))
            {
                using (FileStream fs = new FileStream($"{name}.xml", FileMode.OpenOrCreate))
                {
                    var newSongs = (List<Song>)serializer.Deserialize(fs);
                    Songs = newSongs;
                }
            }
            SongsListChangedEvent?.Invoke(this);
        }
    }
}