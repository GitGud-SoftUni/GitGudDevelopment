using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GitGud.ViewModels
{
    public class ChangeAvatarViewModel
    {
        [Required(ErrorMessage = "Select file or press Cancel, to return to Profile")]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

        public string fileAdress { get; set; }
    }
}
