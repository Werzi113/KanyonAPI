using System.Runtime.CompilerServices;

namespace WebApplication1.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal? Discount { get; set; } 
        public string State { get; set; }

    }
}
