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

            if (currentSong == null)
            {
                return;
            }

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
                .Include("Comments.Likes").FirstOrDefault().Comments;

            List<int> likesIds = new List<int>();

            foreach (var com in currentSongComments)
            {
                likesIds.AddRange(com.Likes.Select(x => x.Id).ToList());
            }

            List<Like> likesToRemove = new List<Like>();

            foreach (var id in likesIds)
            {
                likesToRemove.Add(_context.Likes.Find(id));
            }

            _context.Likes.RemoveRange(likesToRemove);



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
                .Include("Comments.Likes")
                .FirstOrDefault();

            return currentSong;
        }

        public bool SongExists(int sondId)
        {
            bool exists = _context.Songs.Any(x => x.Id == sondId);

            return exists;
        }

        public void AddComment(string userName, string content, int songId)
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

            if (currentUser == null)
            {
                return;
            }

            var songsForCurrentUser = GetAllSongs().Where(s => s.UploaderName == currentUser.UserName);

            foreach (var song in songsForCurrentUser)
            {
                DeleteSong(song.Id);
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

        public void DeleteCategory(int categoryId)
        {
            Category category = _context.Categories.Find(categoryId);

            if (category == null)
            {
                return;
            }

            List<Song> songsForCategory = _context.Songs.Where(s => s.Category.Id == categoryId)
                .ToList();

            foreach (var song in songsForCategory)
            {
                DeleteSong(song.Id);
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public void EditCategory(string newCategoryName, Category category)
        {
            category.Name = newCategoryName;
            _context.SaveChanges();
        }


        public IEnumerable<Song> GetSongByArtist(string artistName)
        {
            IEnumerable<Song> songsByArtist = GetAllSongs().Where(a => a.ArtistName == artistName);

            return songsByArtist;
        }


        public bool UserLikeExists(int commentId, string userName)
        {
            var commentLiked = _context.Comments
                .Where(x => x.Id == commentId)
                .Include(x => x.Likes)
                .FirstOrDefault();


            bool likeExists = false;

            foreach (var like in commentLiked.Likes)
            {
                if (like.User == userName)
                {
                    likeExists = true;
                    break;
                }
            }

            return likeExists;

        }

        public void AddLike(int commentId, string userName)
        {
            var commentLiked = _context.Comments.Find(commentId);

            Like like = new Like();
            like.User = userName;

            commentLiked.Likes.Add(like);

            _context.Entry(commentLiked).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void RemoveLike(int commentId, string userName)
        {
            var commentLiked = _context.Comments
                .Where(x => x.Id == commentId)
                .Include(x => x.Likes)
                .FirstOrDefault();

            var likeToRemove = commentLiked.Likes.Where(x => x.User == userName).FirstOrDefault();

            commentLiked.Likes.Remove(likeToRemove);

            _context.Entry(commentLiked).State = EntityState.Modified;

            _context.Likes.Remove(likeToRemove);

            _context.SaveChanges();
        }

        public User GetUserById(string userId)
        {
            return _context.Users.Find(userId);
        }

        public IEnumerable<User> GetAllAdmins()
        {
            var adminRole = _context.Roles.FirstOrDefault(r => r.Name == "Admin");
            List<User> admins = _context.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains(adminRole.Id)).ToList();

            return admins;
        }

        public bool CommentExists(int? id)
        {
            if (_context.Comments.Any(c => c.Id == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteCommentById(int? id)
        {
            Comment targetedComment = _context.Comments
                .Where(c => c.Id == id)
                .Include(c => c.Likes)
                .FirstOrDefault();

            List<Like> likesToRemove = _context.Comments
                .Where(c => c.Id == id)
                .Include(c => c.Likes)
                .FirstOrDefault().Likes.ToList();



            foreach (var like in likesToRemove)
            {
                targetedComment.Likes.Remove(like);
            }
            _context.Likes.RemoveRange(likesToRemove);
            _context.Entry(targetedComment).State = EntityState.Modified;
            _context.Comments.Remove(targetedComment);

            _context.SaveChanges();

        }

        public IEnumerable<Song> GetAllSongsFromUser(string userName)
        {
            return _context.Songs.Where(s => s.UploaderName == userName).ToList();
        }

        public IEnumerable<Comment> GetCommentsFromUser(string userName)
        {
            return _context.Comments.Where(c => c.UserName == userName).Include(c => c.Likes).ToList();
        }

        
    }
}
