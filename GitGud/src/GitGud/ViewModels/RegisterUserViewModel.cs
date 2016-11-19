using System.ComponentModel.DataAnnotations;

namespace GitGud.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required, DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(3), MaxLength(64), Display(Name = "User Name")]
        public string FullName { get; set; }

        [Required, MinLength(3), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password),Compare(nameof(Password))]
        public string RepeatedPassword { get; set; }
    }
}
