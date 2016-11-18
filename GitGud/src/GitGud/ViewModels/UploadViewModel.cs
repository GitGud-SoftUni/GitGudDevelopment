using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GitGud.ViewModels
{
    public class UploadViewModel
    {
        [Required, DataType(DataType.Upload)]
        public IFormFile MusicFile { get; set; }
        [Required]
        [StringLength(40, MinimumLength =2)]
        public string SongName { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Artist { get; set; }
        [Required]
        public string Tags { get; set; }
    }
}
