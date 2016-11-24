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
                .FirstOrDefault();
            return currentSong;
        }

        public bool SongExists(int sondId)
        {
            bool exists = _context.Songs.Any(x => x.Id == sondId);

            return exists;
        }
    }
}
