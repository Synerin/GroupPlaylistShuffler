using System;
using System.IO;
using System.Collections.Generic;

namespace GroupPlaylistShuffler
{
    public class Playlist
    {
        string PlaylistName;
        List<Song> Songs;

        public Playlist(string PlaylistName)
        {
            this.PlaylistName = PlaylistName;
            Songs = new List<Song>();
        }

        public Playlist(string PlaylistName, List<Song> Songs)
        {
            this.PlaylistName = PlaylistName;
            this.Songs = Songs;
        }
    }
}