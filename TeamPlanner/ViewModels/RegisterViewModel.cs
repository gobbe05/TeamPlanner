using System.ComponentModel.DataAnnotations;

namespace TeamPlanner.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Your name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Your name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
