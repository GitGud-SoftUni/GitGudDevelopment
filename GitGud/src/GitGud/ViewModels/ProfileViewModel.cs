using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GitGud.ViewModels
{
    public class ProfileViewModel
    {
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
       
        [StringLength(20), Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(20), Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        
        [StringLength(20)]
        public string Town { get; set; }
    }
}
