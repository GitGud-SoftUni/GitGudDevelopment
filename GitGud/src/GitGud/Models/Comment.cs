using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public ICollection<Like> Likes { get; set; }

        public Comment()
        {
            this.Likes = new List<Like>();
        }
    }
}
