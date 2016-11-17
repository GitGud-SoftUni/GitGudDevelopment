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
        //Identity package contains every property for a user...
        //        public int Id { get; set; }
        //        public string Username { get; set; } //a.k.a email
        //        public string FullName { get; set; }
        //        public string PasswordHash { get; set; }// a.k.a encrypted password string which is stored in db instead password 
        //        public string Role { get; set; } // user or admin 
        //        public string Salt { get; set; }//a.k.a. encryption key for the password before storing in db


        //public ICollection<Song> Songs { get; set; }
    }
}
