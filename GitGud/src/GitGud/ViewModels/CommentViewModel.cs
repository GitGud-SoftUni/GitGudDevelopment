using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.ViewModels
{
    public class CommentViewModel
    {
        [Required][StringLength(255,MinimumLength = 3)]
        public string Content { get; set; }
    }
}
