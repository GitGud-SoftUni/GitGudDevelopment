using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.ViewModels
{
    public class EditSongViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string SongName { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Artist { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Tags { get; set; }
    }
}
