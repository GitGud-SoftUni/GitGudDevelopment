using System.Collections.Generic;

namespace GitGud.Models
{
    public interface IGitGudRepository
    {
        IEnumerable<Song> GetAllSongs();
        void DeleteSong(int songId);
        IEnumerable<string> GetTopTags();
        IEnumerable<Song> GetSongsByTagName(string tagName);
        Song GetSongById(int songId);
        bool SongExists(int sondId);
    }
}