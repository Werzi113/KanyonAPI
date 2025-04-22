using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public int? DiscountCodeID { get; set; }
        public int? UserID { get; set; }


    }
}
