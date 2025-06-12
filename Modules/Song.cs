using System;

namespace GroupPlaylistShuffler
{
    public class Song
    {
        string SongName;
        string Artist;

        public Song(string songName, string artist)
        {
            this.SongName = songName;
            this.Artist = artist;
        }

        public override string ToString()
        {
            return $"{this.SongName} by {this.Artist}";
        }

        public static Song ProcessSong(string nameAndArtist)
        {
            string[] splitValues = nameAndArtist.Split(new string[] { "," }, StringSplitOptions.None);

            if (splitValues.Length != 2)
            {
                return null;
            }

            return new Song(splitValues[0], splitValues[1]);
        }
    }
}