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
        void AddComment(string userName, string content, int songId);
        IEnumerable<User> GetAllUsers();
        void DeleteUser(string userId);
        IEnumerable<Category> GetAllCategories();
        void AddCategory(string categoryName);
        IEnumerable<Song> SearchSongsByCategory(int categoryId);
        Category SearchCategoryById(int categoryId);
    }
}