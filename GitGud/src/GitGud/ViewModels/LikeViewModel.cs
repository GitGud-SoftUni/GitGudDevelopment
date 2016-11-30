using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.ViewModels
{
    public class LikeViewModel
    {
        [Required]
        public int CommentId { get; set; }
    }
}
