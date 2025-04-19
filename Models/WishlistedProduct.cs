using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("WishlistedProducts")]
    public class WishlistedProduct
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
    }
}
