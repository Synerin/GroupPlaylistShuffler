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

            // Files containing a list of x songs
            string[] textFiles;

            if (args.Length > 0)
            {
                // Read command line arguments for file names
                textFiles = new string[args.Length];

                for (int i = 0; i < args.Length; i++)
                {
                    textFiles[i] = args[i];
                }
            }
            else
            {
                /* File names can alternatively be hardcoded here.
                Will throw a divide-by-0 exception if no args or elements are provided
                */
                textFiles = new string[] {};
            }

            List<string> playlist = new List<string>();

            foreach (string file in textFiles)
            {
                string[] songs = Read(file); // Get all songs in a given file
                string[] shuffled = Playlist.Shuffle(songs); // Shuffle the songs from said file

                // Add each song (in shuffled order) to the list
                foreach (string song in shuffled)
                {
                    playlist.Add(song);
                }

                max = Math.Max(max, songs.Length); // Calculate max
            }

            // Distribute songs evenly in ABC...ABC... order
            string[] distributedPlaylist = Playlist.DistributePlaylist(playlist, textFiles.Length);

            // Shuffle each distributed grouping of N songs for random equality
            string[] finalPlaylist = Playlist.ShufflePlaylist(distributedPlaylist, textFiles.Length);

            // Print finalized playlist to console for confirmation
            foreach (string song in finalPlaylist)
            {
                Console.WriteLine(song);
            }

            /* For translating into an actual playlist,
            I recommend https://www.tunemymusic.com/
            */
            string newFile = "Playlist.txt";

            // Create or overwrite Playlist.txt as needed, add all songs
            File.WriteAllLines(newFile, finalPlaylist);
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