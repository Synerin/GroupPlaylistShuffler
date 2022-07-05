# GroupPlaylistShuffler
A small tool I made in order to more equally shuffle group playlists

## Anticipated Questions
### Why not just shuffle the songs natively?
Native shuffling is unpreserved shuffling. Suppose you begin playing the playlist, then put on something else. This first shuffled order is then lost. Shuffling and playing again will result in a completely different order, thereby making it possible to hear already played songs.

### Isn't shuffling inherently equal?
Shuffling is equal to the extent that any song has a equal opportunity to appear at a given index. However, with multiple users, it becomes more likely that a given user will have to wait longer for any of their songs to appear. The method employed here makes it so that each user has to wait at most 2 * (N - 1) songs between their own, where N is the total number of users. Furthermore, a given user can have **at most** two songs played consecutively.

### What are the limitations of this tool?
Currently, this tool only supports cases where each provided file (representing a user's individual playlist) has an equal number of songs listed. Discrepencies will return an error. This is something that may be resolved in the future.

### What is the Fisher-Yates shuffle?
The Fisher-Yates shuffle, also known as the Knuth shuffle, is an algorithm able to produce every permutation of provided input with equal probability. Originally with a time complexity of O(n<sup>2</sup>), the method provided in main.cs is the modern algorithm, with time complexity of O(n). This method was introduced by Richard Durstenfeld in 1964, and is as follows:
> For i in the range of (N - 1) down to 1,
> - Compute j, where j is a random integer between 0 and i
> - Swap the values at index i and index j

Information sourced and interpreted from the [Fisher-Yates shuffle article on Wikipedia](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle).
