using System.ComponentModel.DataAnnotations;

namespace KarmaWebMVC.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required]
        [StringLength(50)]
        public string? SizeName  { get; set; }
        [Required]

        public int? SizeOder { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}
