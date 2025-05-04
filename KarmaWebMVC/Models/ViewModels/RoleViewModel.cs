using System.ComponentModel.DataAnnotations;

namespace KarmaWebMVC.Models.ViewModels
{
    public class RoleViewModel
    {

        
       public string? Id { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
        public string Name { get; set; }

       
    }
}
