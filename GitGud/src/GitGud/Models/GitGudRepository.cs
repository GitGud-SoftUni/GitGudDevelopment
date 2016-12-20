using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace GitGud.Models
{
    public class GitGudRepository : IGitGudRepository
    {
        private GitGudContext _context;
        private IHostingEnvironment _enviroment;

        public GitGudRepository(GitGudContext context, IHostingEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
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

            var currentSongFavs = _context.Songs.Where(s => s.Id == currentSong.Id)
                .Include(s => s.Favorites).FirstOrDefault().Favorites;

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


            _context.Favs.RemoveRange(currentSongFavs);
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
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .Include(x => x.Favorites)
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

            var getCommentsFromUser = GetCommentsFromUser(currentUser.UserName);

            foreach (var comment in getCommentsFromUser)
            {
                DeleteCommentById(comment.Id);
            }

            var favs = _context.Favs.Where(c => c.UserId == currentUser.Id).ToList();
            var commentLikes = _context.Likes.Where(c => c.User == currentUser.UserName).ToList();




            _context.Likes.RemoveRange(commentLikes);
            _context.Favs.RemoveRange(favs);
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

        public bool UserFavExists(int songId, string userId)
        {
            var song = GetSongById(songId);
            if (song.Favorites.Any(f => f.UserId == userId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddFav(int songId, string userId)
        {
            var song = GetSongById(songId);

            Fav fav = new Fav();
            fav.UserId = userId;
            fav.DateFavorited = DateTime.Now;

            song.Favorites.Add(fav);

            _context.Entry(song).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void RemoveFav(int songId, string userId)
        {
            var song = GetSongById(songId);

            var fav = song.Favorites.Where(s => s.UserId == userId).FirstOrDefault();

            song.Favorites.Remove(fav);

            _context.Favs.Remove(fav);

            _context.Entry(song).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Roles.Include(r => r.Users).ToList();
        }

        public void SaveChangesInDb()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Song> GetHotTracks()
        {
            var dateToday = DateTime.Today;
            var dateTreeDaysAgo = dateToday.AddDays(-3);

            var matchingSongs = GetAllSongs()
                .Where(d => d.DateUploaded >= dateTreeDaysAgo)
                .ToList();

            var hotSongs = new List<Song>();

            foreach (var song in matchingSongs)
            {
                var hotSong = GetSongById(song.Id);
                hotSongs.Add(hotSong);
            }

            hotSongs = hotSongs.OrderByDescending(x => x.DateUploaded).ToList();

            return hotSongs;
        }

        public IEnumerable<Song> GetUserFavs(string userId)
        {
            var favs = _context.Favs
                .Where(c => c.UserId == userId)
                .Include(f => f.Song)
                .ToList();
            var songs = new List<Song>();

            foreach (var fav in favs)
            {
                var song = _context.Songs
                    .Where(s => s.Id == fav.Song.Id)
                    .Include(s => s.Category)
                    .Include(s => s.Favorites)
                    .FirstOrDefault();

                if (song != null)
                {
                    songs.Add(song);
                }
            }
            return songs;
        }

        public void DeleteAvatar(string userId)
        {
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return;
            }

            string avatarFileAddress = Path.GetFullPath("..\\GitGud\\wwwroot\\avatars\\" + user.fileAdress);//Check this

            if (File.Exists(avatarFileAddress))
            {
                try
                {
                    File.Delete(avatarFileAddress);
                }
                catch (Exception)
                {

                }
            }

            user.fileAdress = null;
            _context.SaveChanges();
        }

        public bool SongDuplicateExists(string songName, string artistName)
        {
            var songs = GetAllSongs();

            var songAlreadyExists = songs.Any(
                x => x.Name.ToLower() == songName.ToLower()
                &&
                x.ArtistName.ToLower() == artistName.ToLower()
                );

            return songAlreadyExists;
        }

        public bool DuplicateIsThisSong(int songId, string songName, string artistName)
        {
            var songs = GetAllSongs();

            var songDuplicate = songs.Where(x => x.Name.ToLower() == songName.ToLower()
                &&
                x.ArtistName.ToLower() == artistName.ToLower()
                ).FirstOrDefault();

            if (songDuplicate == null)
            {
                return false;
            }

            if (songDuplicate.Id == songId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public User GetUserByUsername(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
        }

        public void EditSong(int songId, string artistName, string songName, string categoryId, string tags)
        {
            var song = GetSongById(songId);

            List<Tag> oldTags = song.Tags.ToList();

            List<string> newTagsString = tags.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();

            List<Tag> newTags = new List<Tag>();

            foreach (var tag in newTagsString)
            {
                newTags.Add(new Tag { Name = tag });
            }

            Category category = SearchCategoryById(int.Parse(categoryId));

            foreach (var tag in oldTags)
            {
                song.Tags.Remove(tag);
            }

            _context.Tags.RemoveRange(oldTags);

            var directory = Path.Combine(_enviroment.WebRootPath, "uploads");

            var oldFileName = song.fileAdress;
            var newFileName = $"{songName} - {artistName}.mp3";

            if (oldFileName != newFileName)
            {
                var oldPath = Path.Combine(directory, oldFileName);
                var newPath = Path.Combine(directory, newFileName);

                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }
                File.Move(oldPath, newPath);
            }



            song.Tags = newTags;
            song.ArtistName = artistName;
            song.Name = songName;
            song.Category = category;
            song.fileAdress = newFileName;
            _context.Entry(song).State = EntityState.Modified;

            _context.SaveChanges();
        }
    }
}
