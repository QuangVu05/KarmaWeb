using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebMVC.Models
{
    public class Order
    {
        [Key] 
        public int OrderId { get; set; }
        [Required]
        [Display(Name ="Full Name")]
        public string?  FullName { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Note { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        [ForeignKey("OrderStatusId")] //Khoa ngoài 
        public int OrderStatusId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
       
        public List<OrderItems>? OrderItems { get; set; }
        // Thêm thuộc tính UserId để tạo mối quan hệ với User
        public string? UserId { get; set; } // Khóa ngoại đến AspNetUsers
        public AppUserModel? User { get; set; } // Điều hướng đến User

        public decimal TotalPriceOder => OrderItems?.Sum(p => p.ProductPrice * p.ProductQuantity) ?? 0;
		


	}
}
