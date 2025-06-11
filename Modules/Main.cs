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

            string[] textFiles = Directory.GetFiles(playlistDir, "*.txt");

            // Files containing a list of x songs
            Playlist unifiedPlaylist = new Playlist("Playlist.txt");

            foreach (string file in textFiles)
            {
                string[] songArray = Read(file); // Get all songs in a given file
                List<Song> songList = new List<Song>();

                foreach (string listedSong in songArray)
                {
                    Song song = Song.ProcessSong(listedSong);
                    songList.Add(song);
                }

                Playlist localPlaylist = new Playlist(songList);
                localPlaylist.Shuffle(); // Shuffle the songs from said file

                max = Math.Max(max, songArray.Length); // Calculate max

                unifiedPlaylist.AddSongs(localPlaylist.Songs); // Add to unified playlist
            }

            // Distribute songs evenly in ABC...ABC... order
            unifiedPlaylist.DistributePlaylist(textFiles.Length);

            // Shuffle each distributed grouping of N songs for random equality
            Playlist finalPlaylist = Playlist.ShuffleSections(unifiedPlaylist.Songs, textFiles.Length);

            // Print finalized playlist to console for confirmation
            foreach (Song song in finalPlaylist.Songs)
            {
                Console.WriteLine(song);
            }

            /* For translating into an actual playlist,
            I recommend https://www.tunemymusic.com/
            */
            string newPlaylistFile = Path.Combine(binDir, "../../CreatedPlaylists/Playlist.txt");

            // Create or overwrite Playlist.txt as needed, add all songs
            File.WriteAllLines(newPlaylistFile, finalPlaylist.FormatForWriting());
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