using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public DateTime DateUploaded { get; set; }
        //public User Uploader { get; set; }

        public IFormFile MusicFile { get; set; }

        public ICollection<Tag> Tags { get; set; }

    }
}
