using NAudio.Wave;
using Plugin.Maui.Audio;

namespace Music.MusicDomain
{
    public class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        /// <summary>
        /// Creates a new Loop stream
        /// </summary>
        /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
        /// or else we will not loop to the start again.</param>
        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            EnableLooping = true;
        }

        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool EnableLooping { get; set; }

        /// <summary>
        /// Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        /// <summary>
        /// LoopStream simply returns
        /// </summary>
        public override long Length
        {
            get { return sourceStream.Length; }
        }

        /// <summary>
        /// LoopStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }

    public class Track
    {
        public int Id { get; }
        public string Title { get; }
        public int Type { get; } // 1 - drums, 2 - instrument, 3 - fx, 4 - voice
        public byte[] SongData { get; }

        private readonly IAudioManager audioManager;
        private IAudioPlayer audioPlayer;

        //private WaveOutEvent waveOut;
        //private WaveStream waveStream;
        //private long timestamp = 0;

        public Track(int id, string title, int type, byte[] songData)
        {
            Id = id;
            Title = title;
            Type = type;
            SongData = songData;
            
            audioManager = AudioManager.Current;
            

            //waveOut = new WaveOutEvent();
        }

        public void Play()
        {
            if (SongData.Length == 0 || SongData == null)
            {
                return;
            }
            /*if (waveOut.PlaybackState == PlaybackState.Stopped )
            {
                //read the wav data from the byte array
                waveStream = new WaveFileReader(new MemoryStream(songData));
                //WaveStream waveStream = new Mp3FileReader(new MemoryStream(audioData));
                LoopStream loop = new LoopStream(waveStream);
                loop.EnableLooping = true;

                waveOut.Dispose();
                //waveOut.Init(loop);
                waveOut.Init(waveStream);
                waveOut.Play();
            }*/
            if (audioPlayer != null)
            {
                audioPlayer.Stop();
            }

            audioPlayer = audioManager.CreatePlayer(new MemoryStream(SongData));
            audioPlayer.Loop = true;
            audioPlayer.Play();
        }

        public void Stop()
        {
            //timestamp = 0;
            //waveOut.Dispose();
            //if(waveStream != null)
            //{
            //    waveStream.Dispose();
            //}
            if (audioPlayer != null)
            {
                audioPlayer.Stop();
            }
        }

        //public PlaybackState GetPlaybackState()
        //{
        //    return waveOut.PlaybackState;
        //}
    }
}