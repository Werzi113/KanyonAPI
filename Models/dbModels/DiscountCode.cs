using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("DiscountCodes")]
    public class DiscountCode
    {
        [Key]
        public int CodeID { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Discount { get; set; }

    }
}
