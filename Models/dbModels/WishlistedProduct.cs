using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("WishlistedProducts")]
    [PrimaryKey(nameof(ProductID), nameof(UserID))]
    public class WishlistedProduct
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
    }
}
