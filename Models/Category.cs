using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Categories")]
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentID { get; set; }
    }
}
