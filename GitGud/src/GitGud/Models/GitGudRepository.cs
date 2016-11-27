using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GitGud.Models
{
    public class GitGudRepository : IGitGudRepository
    {
        private GitGudContext _context;

        public GitGudRepository(GitGudContext context)
        {
            _context = context;
        }

        public IEnumerable<Song> GetAllSongs()
        {
            return _context.Songs.ToList();
        }

        public void DeleteSong(int songId)
        {
            Song currentSong = _context.Songs.Find(songId);
            string songFileAddress = Path.GetFullPath("..\\GitGud\\wwwroot\\uploads\\" + currentSong.fileAdress);

            if (File.Exists(songFileAddress))
            {
                try
                {
                    File.Delete(songFileAddress);
                }
                catch (Exception)
                {

                }
            }

            //Get Tags for current song
            var currentSongTags = _context.Songs.Where(s => s.Id == currentSong.Id)
                .Include(s => s.Tags).FirstOrDefault().Tags;

            var currentSongComments = _context.Songs.Where(s => s.Id == currentSong.Id)
                .Include(s => s.Comments).FirstOrDefault().Comments;

            _context.Comments.RemoveRange(currentSongComments);
            _context.Tags.RemoveRange(currentSongTags);
            _context.Songs.Remove(currentSong);
            _context.SaveChanges();
        }

        public IEnumerable<string> GetTopTags()
        {
            var allTags = _context.Tags.ToList();

            Dictionary<string, int> tagsCounter = new Dictionary<string, int>();

            foreach (var tag in allTags)
            {
                if (tagsCounter.ContainsKey(tag.Name))
                {
                    tagsCounter[tag.Name] += 1;
                }
                else
                {
                    tagsCounter.Add(tag.Name, 1);
                }
            }

            List<string> topTags = tagsCounter.OrderByDescending(x => x.Value).Take(25).Select(y => y.Key).ToList();

            return topTags;
        }

        public IEnumerable<Song> GetSongsByTagName(string tagName)
        {

            List<Song> songsByTagName = new List<Song>();

            List<Song> allSongs = GetAllSongs().ToList();

            foreach (var song in allSongs)
            {
                List<Tag> tagsForSong = _context.Songs.Where(s => s.Id == song.Id)
                    .Include(s => s.Tags).FirstOrDefault().Tags.ToList();

                if (tagsForSong.Any(t => t.Name == tagName))
                {
                    songsByTagName.Add(song);
                }
            }

            return songsByTagName;
        }

        public Song GetSongById(int songId)
        {

            Song currentSong = _context.Songs
                .Where(x => x.Id == songId)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .FirstOrDefault();
            return currentSong;
        }

        public bool SongExists(int sondId)
        {
            bool exists = _context.Songs.Any(x => x.Id == sondId);

            return exists;
        }

        public void AddComment(string userName, string content, int songId )
        {
            Song songCommented = GetSongById(songId);
            Comment comment = new Comment();
            comment.Content = content;
            comment.UserName = userName;
            songCommented.Comments.Add(comment);

            _context.Entry(songCommented).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void DeleteUser(string userId)
        {
            User currentUser = _context.Users.Find(userId);
            var songsForCurrentUser = GetAllSongs().Where(s => s.UploaderName == currentUser.UserName);

            //Delete all tags and comments for songs that uploaded by currentUser
            foreach (var song in songsForCurrentUser)
            {
                var currentSongTags = _context.Songs.Where(s => s.Id == song.Id)
                    .Include(s => s.Tags).FirstOrDefault().Tags;

                var currentSongComments = _context.Songs.Where(s => s.Id == song.Id)
                    .Include(s => s.Comments).FirstOrDefault().Comments;

                //Check if song file exist, if so delete this song on local level first
                string songFileAddress = Path.GetFullPath("..\\GitGud\\wwwroot\\uploads\\" + song.fileAdress);

                if (File.Exists(songFileAddress))
                {
                    try
                    {
                        File.Delete(songFileAddress);
                    }
                    catch (Exception)
                    {

                    }
                }

                _context.Tags.RemoveRange(currentSongTags);
                _context.Comments.RemoveRange(currentSongComments);
                _context.Songs.Remove(song);
            }

            _context.Users.Remove(currentUser);
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public void AddCategory(string categoryName)
        {
            Category category = new Category()
            {
                Name = categoryName
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public IEnumerable<Song> SearchSongsByCategory(int categoryId)
        {

            List<Song> songsForCategories = _context.Songs.Where(s => s.Category.Id == categoryId)
                .ToList();

            return songsForCategories;
        }


        public Category SearchCategoryById(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

    }
}
