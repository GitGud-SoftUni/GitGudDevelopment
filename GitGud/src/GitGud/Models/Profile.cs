using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GitGud.Models
{
    public class Profile
    {
        public Profile()
        {
            this.Comments = new List<Comment>();
            this.Songs = new List<Song>();
        }
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime Birthday { get; set; }
           
        public string Town { get; set; } 
        public string fileAdress { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Comment> Comments { get; set; }
        [Key]
        public User UserId { get; set; }
        
    }
}
