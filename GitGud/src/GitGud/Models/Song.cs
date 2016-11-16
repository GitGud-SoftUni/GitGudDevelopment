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
        public string Name { get; set; }
        public string ArtistName { get; set; }
<<<<<<< HEAD

        public DateTime DateUploaded
        {
            get
            {
                return DateTime.Now;
            }
        }
        public string UploaderName { get; set; }

        //public IFormFile MusicFile { get; set; }
=======
        public DateTime DateUploaded { get; set; }
        public User Uploader { get; set; }
        public string UploaderName { get; set; }
        public IFormFile MusicFile { get; set; }
>>>>>>> b31129709ba00469dbe555a22505ecbcee2100c4

        public ICollection<Tag> Tags { get; set; }

    }
}
