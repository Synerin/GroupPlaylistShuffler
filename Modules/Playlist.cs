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

        /// <summary>
        /// Randomly reorder given generic array based on Fisher-Yates shuffle
        /// </summary>
        /// <returns>
        /// A shuffled array
        /// </returns>
        /// <param name="input">
        /// A generic array
        /// </param>
        public static T[] Shuffle<T>(T[] input)
        {
            T[] output = input;

            int n = input.Length;
            Random rand = new Random();

            while (n > 1)
            {
                int k = rand.Next(n--);

                // Cannot use tuple assignment for Random, must assign a temp variable
                T temp = output[n];
                output[n] = output[k];
                output[k] = temp;
            }

            return output;
        }

        /// <summary>
        /// Distribute songs from each user in ABC...ABC... order
        /// </summary>
        /// <returns>
        /// A string array of songs evenly distributed between all users
        /// </returns>
        /// <param name="input">
        /// A list of strings where each element is a song
        /// </param>
        /// <param name="totalUsers">
        /// The number of files provided in main()
        /// </param>
        public static string[] DistributePlaylist(List<string> input, int totalUsers)
        {
            if (totalUsers < 1)
            {
                return null;
            }

            int totalSongs = input.Count;
            string[] output = new string[totalSongs];

            int songsPerUser = totalSongs / totalUsers;
            /* Assign each song from a user's list to an index based on the formula:
            Index = U + N * S,
            where
              U is the given user,
              N is the total number of users,
              and S is the index of the given song in the given user's list
            */
            for (int i = 0; i < totalSongs; i++)
            {
                int user = i / songsPerUser; // Calculate user
                int songIndex = i % songsPerUser; // Calculate song's local index

                output[user + totalUsers * songIndex] = input[i];
            }

            return output;
        }

        /// <summary>
        /// Read input file and parse each newline as a song
        /// </summary>
        /// <remarks>
        /// The intention of this method is to guarantee that each user will have
        /// a song played in each group of N, where N is the total number of users.
        /// This is to reduce predictability (i.e. a song from user B always plays
        /// after a song from user A), while also providing equal opportunity.
        /// While it is possible for user X to have up to 2 * (N - 1) songs between
        /// their own (e.g. ABCD-DBCA), it eliminates the possibility of an even
        /// larger gap (e.g. ABCD-DBCD-CBDA). Furthermore, a given user can have
        /// at most two of their songs played consecutively.
        /// </remarks>
        /// <returns>
        /// A string array of songs shuffled in a random but fair order
        /// </returns>
        /// <param name="input">
        /// A string array of evenly distributed songs
        /// </param>
        /// <param name="totalUsers">
        /// The number of files provided in main()
        /// </param>
        public static string[] ShufflePlaylist(string[] input, int totalUsers)
        {
            if (totalUsers < 1)
            {
                return null;
            }

            string[] output = new string[input.Length];
            int songsPerUser = input.Length / totalUsers;

            for (int i = 0; i < songsPerUser; i++)
            {
                // Cut out a subsection of N songs from input
                string[] subsection = new string[totalUsers];
                Array.Copy(input, i * totalUsers, subsection, 0, totalUsers);

                // Shuffle the subsection
                Shuffle(subsection);

                // Copy the shuffled subsection back into the output
                Array.Copy(subsection, 0, output, i * totalUsers, totalUsers);
            }

            return output;
        }
    }
}