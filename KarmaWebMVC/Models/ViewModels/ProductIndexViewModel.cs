namespace KarmaWebMVC.Models.ViewModels
{
    public class ProductIndexViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Query { get; set; }
        public string BrandSlug { get; set; }
        public string CategorySlug { get; set; }
    }
}
