namespace WebApplication1.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string? Decision { get; set; }
        public int? ParentID { get; set; }
    }
}
