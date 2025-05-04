using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KarmaWebMVC.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(60)]
        public string? ProductName { get; set; }

        [StringLength(500)]
        public string? ProductDescription { get; set; }
        [Required]
        public decimal? ProductPrice { get; set; }
        [Required]
        public int? ProductQuantity { get; set; }
        [Required]

        [StringLength(50)]
        public string? ProductImage { get; set; }

        [ForeignKey("CategoryId")] //Khoa ngoài đến Categrory
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [ForeignKey("BrandId")] //Khoa nngoài đến Brand
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

        [ForeignKey("SizeId")] //Khoa nngoài đến Size
        public int SizeId { get; set; }
        public Size? Size { get; set; }
        // Khóa ngoại tham chiếu đến bảng ProductStatus
        [ForeignKey("ProductStatus")]
        public int ProductStatusId { get; set; }  // Khóa ngoại

        // Tính năng điều hướng để truy cập trạng thái sản phẩm
        public ProductStatus? ProductStatus { get; set; }

        // Thêm thời gian cho sản phẩm
        [DataType(DataType.DateTime)]
        [Column("AddedDate")]
        public DateTime? Date { get; set; } = DateTime.Now;
        public ICollection<Cart>? CartItems { get; set; } // Quan hệ một-nhiều với CartItem


    }
}
