namespace WebApplication1.Models.DTO.Categories
{
    public class CategoryPreviewDTO
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public int? ParentID { get; set; }

        public string? Parent {  get; set; }
    }
}
