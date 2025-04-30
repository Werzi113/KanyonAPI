using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.Wishlist
{
    public class WishlistProductDTO
    {
        public int ProductID { get; set; }

        public string ImageURL { get; set; }

        public decimal Discount { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }
    }
}