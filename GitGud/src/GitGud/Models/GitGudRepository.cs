using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
