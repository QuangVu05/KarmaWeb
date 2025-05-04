using KarmaWebMVC.Models;

namespace KarmaWebMVC.Models.ViewModels
{
    public class ProductViewModel
    {
        public List<Product> Top2Products { get; set; }
        public List<Product> AllProducts { get; set; }
    }
}
