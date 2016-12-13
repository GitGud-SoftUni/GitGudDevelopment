using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.Models
{
    public class Fav
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Song Song { get; set; }
        public DateTime DateFavorited { get; set; }
    }
}
