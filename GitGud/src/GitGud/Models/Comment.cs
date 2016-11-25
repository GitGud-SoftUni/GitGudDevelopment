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
        public DateTime Date
        {
            get
            {
                return DateTime.Now;
            }
        }
        public string Content { get; set; }
        public int Likes { get; set; }

        public Comment()
        {
            this.Likes = 0;
        }
    }
}
