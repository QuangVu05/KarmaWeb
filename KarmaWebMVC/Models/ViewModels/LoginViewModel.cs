using System.ComponentModel.DataAnnotations;

namespace KarmaWebMVC.Models.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is required.")]
        public string? UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }
		public string? ReturnUrl { get; set; }



	}
}
