using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GitGud.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Comments = new List<Comment>();
            this.Songs = new List<Song>();
            this.Likes = new List<Like>();
            this.Birthday = new DateTime();
            this.Favs = new List<Fav>();
            this.FavSongs = new List<Song>();
            this.Age = DateTime.Now.Year - Birthday.Year;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public int Age { get; set; }
        public string Town { get; set; }
        public string fileAdress { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Fav> Favs { get; set; }
        public ICollection<Song> FavSongs { get; set; }
    }
}
