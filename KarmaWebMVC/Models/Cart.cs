namespace KarmaWebMVC.Models
{
	public class Cart
	{
		public int CartId { get; set; }
		public string CartName { get; set; }
		public string CartImage { get; set; }
		public decimal CartPrice { get; set; }
		public int CartQuantity { get; set; }
		public decimal CartTotalPrice => CartPrice * CartQuantity;
        public string? UserId { get; set; } // Khóa ngoại đến AspNetUsers
        public AppUserModel? User { get; set; } // Điều hướng đến User


    }
}
