using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Warehouses")]
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; } 

    }
}
