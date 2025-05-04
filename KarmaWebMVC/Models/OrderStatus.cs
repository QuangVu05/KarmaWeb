using System.ComponentModel.DataAnnotations;

namespace KarmaWebMVC.Models
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        [Required]
        public string? StatusName { get; set; }
     
    }
}
