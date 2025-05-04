using System.ComponentModel.DataAnnotations;

namespace KarmaWebMVC.Models
{
    public class ProductStatus
    {
        [Key]
        public int ProductStatusId { get; set; }  // Khóa chính
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }  // Tên trạng thái sản phẩm
    }
}
