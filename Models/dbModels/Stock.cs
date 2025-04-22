using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Stocks")]
    public class Stock
    {
        [Key]
        public int StockID { get; set; }
        public int Amount { get; set; }
        public int ProductID { get; set; }
        public DateTime Date {  get; set; }
        public int WarehouseID { get; set; }
    }
}
