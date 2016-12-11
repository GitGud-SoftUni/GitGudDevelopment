using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitGud.Models;

namespace GitGud.ViewModels
{
    public class EditUserViewModel
    {
        public User User { get; set; }

        [MinLength(3), DataType(DataType.Password)]
        public string Password { get; set; }


        [DisplayName("Confirm Password")]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public IList<CheckUserRolesViewModel> Roles { get; set; }
    }
}
