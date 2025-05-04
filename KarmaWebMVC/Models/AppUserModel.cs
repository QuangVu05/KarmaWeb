using Microsoft.AspNetCore.Identity;

namespace KarmaWebMVC.Models
{
    public class AppUserModel:IdentityUser
    {
        public string? Token { get; set; }
        // Thêm danh sách các đơn hàng của người dùng
        public List<Order> Orders { get; set; } = new List<Order>(); // Khóa ngoại tới Order
    }
}
