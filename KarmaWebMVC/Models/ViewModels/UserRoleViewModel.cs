using System.ComponentModel.DataAnnotations;

namespace KarmaWebMVC.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string? SelectedRole { get; set; }   

        public List<string> AvailableRoles { get; set; } = new List<string>(); // Danh sách tất cả vai trò
    }
}
