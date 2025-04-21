using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.Warehouses
{
    public class CreateWarehouseDTO
    {
        [Required()]
        [StringLength(100)]
        public string City { get; set; }

        [Required()]
        [StringLength(100)]
        public string Country { get; set; }

        [Required()]
        [StringLength(6)]
        [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Wrong postcode format")]
        public string PostCode { get; set; }

        [Required()]
        [StringLength(100)]
        public string Street { get; set; }

        public Warehouse CreateWarehouse()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.WarehouseId = 0;
            warehouse.City = City;
            warehouse.Country = Country;
            warehouse.PostCode = PostCode;
            warehouse.Street = Street;

            return warehouse;
        }
    }
}