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
        public DateTime DateUploaded { get; set; }
        public string UploaderName { get; set; }
        public string fileAdress { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Fav> Favorites { get; set; }
        public int Downloads { get; set; } = 0;


        public Song()
        {
            this.Comments = new List<Comment>();
            this.Favorites = new List<Fav>();
        }

    }
}
