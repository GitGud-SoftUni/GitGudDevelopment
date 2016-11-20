using System.ComponentModel.DataAnnotations;

namespace GitGud.ViewModels
{
    public class LoginViewModel
    {
        [Required, Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        //to keep session or not
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; } //this is the url from which the user gets login form, and where will 
                                              //be redirected again when loggs in
    }
}
