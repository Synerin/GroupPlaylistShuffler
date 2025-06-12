using System;
using System.IO;
using System.Collections.Generic;

namespace GroupPlaylistShuffler
{
    class GroupPlaylistShuffler
    {
        static void Main(string[] args)
        {
            // Keep track of most songs in one file
            int max = 0;

            string binDir = Directory.GetCurrentDirectory();
            string playlistDir = Path.Combine(binDir, "../../UserPlaylists");

            string[] textFiles = Directory.GetFiles(playlistDir);

            Playlist unifiedPlaylist = new Playlist("FinalPlaylist");

            Dictionary<int, Playlist> userPlaylists = new Dictionary<int, Playlist>();

            foreach (string file in textFiles)
            {
                string[] songArray = Read(file);
                List<Song> songList = new List<Song>();

                foreach (string listedSong in songArray)
                {
                    Song song = Song.ProcessSong(listedSong);
                    songList.Add(song);
                }

                Playlist localPlaylist = new Playlist(songList);
                localPlaylist.Shuffle();

                userPlaylists.TryGetValue(textFiles.Length, out Playlist playListN);

                if (playListN == null)
                {
                    playListN = new Playlist($"Playlist{localPlaylist.Songs.Count}");
                    userPlaylists.Add(textFiles.Length, playListN);
                }
                else
                {
                    playListN.AddSongs(localPlaylist.Songs);
                    //userPlaylists[textFiles.Length] = playListN;
                }

                max = Math.Max(max, songArray.Length); // Nothing implemented for this yet
            }

            foreach (KeyValuePair<int, Playlist> kvp in userPlaylists)
            {
                Playlist playlistX = kvp.Value;
                int users = playlistX.Songs.Count / kvp.Key;

                // Distribute songs evenly in ABC...ABC... order
                playlistX.DistributePlaylist(users);
                
                Playlist shuffledPlaylistX = Playlist.ShuffleSections(playlistX, users);

                unifiedPlaylist.AddSongs(playlistX);
            }

            // Print finalized playlist to console for confirmation
            foreach (Song song in unifiedPlaylist.Songs)
            {
                Console.WriteLine(song);
            }

            /* For translating into an actual playlist,
            I recommend https://www.tunemymusic.com/
            */
            string newPlaylistFile = Path.Combine(binDir, $"../../CreatedPlaylists/{unifiedPlaylist.PlaylistName}.txt");

            // Create or overwrite Playlist.txt as needed, add all songs
            File.WriteAllLines(newPlaylistFile, unifiedPlaylist.FormatForWriting());
        }

        /// <summary>
        /// Read input file and parse each newline as a song
        /// </summary>
        /// <returns>
        /// A string array where each element is a song from input
        /// </returns>
        /// <param name="input">
        /// A .txt file containing a list of songs
        /// </param>
        public static string[] Read(string input)
        {
            return System.IO.File.ReadAllLines(input);
        }
    }
}