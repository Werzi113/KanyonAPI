using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
