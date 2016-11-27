using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.ViewModels
{
    public class CategoryViewModel
    {
        [Required, MinLength(2)]
        public string CategoryName { get; set; }
    }
}
