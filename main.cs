using System;
using System.IO;
using System.Collections.Generic;

class GroupPlaylistShuffler
{
  static void Main (string[]args)
  {

    // Files containing a list of x songs
    string[]textFiles;

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
	textFiles = new string[]
	{

	};
      }

    List < string > playlist = new List < string > ();

    foreach (string file in textFiles)
    {
      string[]songs = Read (file);	// Get all songs in a given file
      string[]shuffled = Shuffle (songs);	// Shuffle the songs from said file

      // Add each song (in shuffled order) to the list
      foreach (string song in shuffled)
      {
	playlist.Add (song);
      }
    }

    // Distribute songs evenly in ABC...ABC... order
    string[]distributedPlaylist =
      DistributePlaylist (playlist, textFiles.Length);

    // Shuffle each distributed grouping of N songs for random equality
    string[]finalPlaylist =
      ShufflePlaylist (distributedPlaylist, textFiles.Length);

    // Print finalized playlist to console for confirmation
    foreach (string song in finalPlaylist)
    {
      Console.WriteLine (song);
    }

    /* For translating into an actual playlist,
       I recommend https://www.tunemymusic.com/
     */
    string newFile = "Playlist.txt";

    // Create or overwrite Playlist.txt as needed, add all songs
    File.WriteAllText (newFile, "");
    File.AppendAllLines (newFile, finalPlaylist);
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
  public static string[] Read (string input)
  {
    return System.IO.File.ReadAllLines (input);
  }

  /// <summary>
  /// Randomly reorder given string array based on Fisher-Yates shuffle
  /// </summary>
  /// <returns>
  /// A shuffled string array
  /// </returns>
  /// <param name="input">
  /// A string array where each element is a song
  /// </param>
  public static string[] Shuffle (string[]input)
  {
    string[]output = input;

    int n = input.Length;
    Random rand = new Random ();

    while (n > 1)
      {
	int k = rand.Next (n--);

	// Cannot use tuple assignment for Random, must assign a temp variable
	string temp = output[n];
	output[n] = output[k];
	output[k] = temp;
      }

    return output;
  }

  /// <summary>
  /// Distribute songs from each user in ABC...ABC... order
  /// </summary>
  /// <remarks>
  /// Number of songs from each user must be equal, may be adjusted later
  /// </remarks>
  /// <returns>
  /// A string array of songs evenly distributed between all users
  /// </returns>
  /// <param name="input">
  /// A list of strings where each element is a song
  /// </param>
  /// <param name="totalUsers">
  /// The number of files provided in main()
  /// </param>
  public static string[] DistributePlaylist (List < string > input,
					     int totalUsers)
  {
    /* Quick check to see if each user has an equal number of songs,
       not a guaranteed method though, to be adjusted later
     */
    if (input.Count % totalUsers != 0)
      {
	Console.WriteLine ("Unequal number of songs between users provided.");
	return null;
      }

    int totalSongs = input.Count;
    int songsPerUser = totalSongs / totalUsers;

    string[]output = new string[totalSongs];

    /* Assign each song from a user's list to an index based on the formula:
       Index = U + N * S,
       where
       U is the given user,
       N is the total number of users,
       and S is the index of the given song in the given user's list
     */
    for (int i = 0; i < totalSongs; i++)
      {
	int user = i / songsPerUser;	// Calculate user
	int songIndex = i % songsPerUser;	// Calculate song's local index

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
  public static string[] ShufflePlaylist (string[]input, int totalUsers)
  {
    string[]output = new string[input.Length];
    int songsPerUser = input.Length / totalUsers;

    for (int i = 0; i < songsPerUser; i++)
      {
	// Cut out a subsection of N songs from input
	string[]subsection = new string[totalUsers];
	Array.Copy (input, i * totalUsers, subsection, 0, totalUsers);

	// Shuffle the subsection
	Shuffle (subsection);

	// Copy the shuffled subsection back into the output
	Array.Copy (subsection, 0, output, i * totalUsers, totalUsers);
      }

    return output;
  }
}
