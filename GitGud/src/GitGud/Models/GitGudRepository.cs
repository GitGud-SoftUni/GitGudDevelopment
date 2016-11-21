using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                File.Delete(songFileAddress);
            }

            //Get Tags for current song
            var currentSongTags = _context.Songs.Where(s => s.Id == currentSong.Id)
                .Include(s => s.Tags).FirstOrDefault().Tags;

            _context.Tags.RemoveRange(currentSongTags);
            _context.Songs.Remove(currentSong);
            _context.SaveChanges();
        }
    }
}
