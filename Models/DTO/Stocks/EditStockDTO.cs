using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.DTO.Stocks
{
    public class EditStockDTO
    {
        [Required()]
        public int ProductID { get; set; }

        [Required()]
        public DateTime Date { get; set; }

        [Required()]
        public int WarehouseID { get; set; }

        [Required()]
        public int Amount { get; set; }
    }
}