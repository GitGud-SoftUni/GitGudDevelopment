using System.Collections.Generic;

namespace GitGud.Models
{
    public interface IGitGudRepository
    {
        IEnumerable<Song> GetAllSongs();
    }
}