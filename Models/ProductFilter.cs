namespace WebApplication1.Models
{
    public class ProductFilter
    {
        public string? Name { get; set; }
        public int? CategoryID { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinRating { get; set; }
    }
}
