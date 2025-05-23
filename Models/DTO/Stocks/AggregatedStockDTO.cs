using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.DTO.Stocks
{
    public class AggregatedStockDTO
    {
        public int ProductID { get; set; }

        public int WarehouseID { get; set; }

        public int Amount { get; set; }

        public string Name { get; set; }
    }
}