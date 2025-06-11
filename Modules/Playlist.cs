using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GroupPlaylistShuffler
{
    public class Playlist
    {
        public string PlaylistName { get; set; }
        public List<Song> Songs { get; set; }

        public Playlist(string PlaylistName)
        {
            this.PlaylistName = PlaylistName;
            this.Songs = new List<Song>();
        }

        public Playlist(List<Song> Songs)
        {
            this.PlaylistName = "Playlist";
            this.Songs = Songs;
        }

        public Playlist(string PlaylistName, List<Song> Songs)
        {
            this.PlaylistName = PlaylistName;
            this.Songs = Songs;
        }

        public void AddSong(Song song)
        {
            this.Songs.Add(song);
        }

        /// <summary>
        /// Add a list of songs to this playlist
        /// </summary>
        /// <param name="songs">List of songs to add</param>
        public void AddSongs(List<Song> songs)
        {
            foreach (Song song in songs)
            {
                this.AddSong(song);
            }
        }

        /// <summary>
        /// Add all songs from a given playlist to this playlist
        /// </summary>
        /// <param name="playlist">Provided playlist</param>
        public void AddSongs(Playlist playlist)
        {
            this.AddSongs(playlist.Songs);
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
        public void Shuffle()
        {
            List<Song> input = this.Songs;

            List<Song> output = input;

            int n = input.Count;
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            while (n > 1)
            {
                int k = rand.Next(n--);

                // Cannot use tuple assignment for Random, must assign a temp variable
                Song temp = input[n];
                output[n] = output[k];
                output[k] = temp;
            }

            this.Songs = output;
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
        public void DistributePlaylist(int totalUsers)
        {
            if (totalUsers < 1)
            {
                // throw or something
            }

            int totalSongs = this.Songs.Count;
            List<Song> output = new List<Song>(new Song[totalSongs]);

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

                output[user + totalUsers * songIndex] = this.Songs[i];
            }

            this.Songs = output;
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
        public static Playlist ShuffleSections(List<Song> input, int totalUsers)
        {
            if (totalUsers < 1)
            {
                return null;
            }

            List<Song> output = new List<Song>();
            int songsPerUser = input.Count / totalUsers;

            for (int i = 0; i < songsPerUser; i++)
            {
                // Cut out a subsection of N songs from input
                Song[] subsection = new Song[totalUsers];
                Array.Copy(input.ToArray(), i * totalUsers, subsection, 0, totalUsers);

                Playlist subsectionPlaylist = new Playlist(new List<Song>(subsection));

                // Shuffle the subsection
                subsectionPlaylist.Shuffle();

                // Copy the shuffled subsection back into the output
                //Array.Copy(subsection, 0, output.ToArray(), i * totalUsers, totalUsers);
                output.AddRange(subsectionPlaylist.Songs);
            }

            Playlist outputPlaylist = new Playlist(output);

            return outputPlaylist;
        }

        public string[] FormatForWriting()
        {
            string[] strings = new string[this.Songs.Count];

            for (int i = 0; i < this.Songs.Count; i++)
            {
                strings[i] = this.Songs[i].ToString();
            }

            return strings;
        }
    }
}