using System.ComponentModel.DataAnnotations;

namespace KarmaWebMVC.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
    }
}
