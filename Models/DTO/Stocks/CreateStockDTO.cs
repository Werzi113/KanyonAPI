using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.DTO.Stocks
{
    public class CreateStockDTO
    {
        [Required()]
        public int ProductID { get; set; }

        [Required()]
        public DateTime Date { get; set; }

        [Required()]
        public int WarehouseID { get; set; }

        [Required()]
        public int Amount { get; set; }

        public Stock CreateStock()
        {
            Stock stock = new Stock();

            stock.ProductID = ProductID;
            stock.Date = Date;
            stock.WarehouseID = WarehouseID;
            stock.Amount = Amount;
            stock.StockID = 0;
            stock.Date = DateTime.Now;

            return stock;
        }
    }
}