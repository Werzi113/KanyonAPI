using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Enums;
using WebApplication1.Models.DTO.Warehouses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {

        private MyContext _context = new MyContext();

        [HttpGet("{id}")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetWarehouseByID(int id)
        {
            Warehouse warehouse = _context.Warehouses.Find(id);

            if (warehouse == null)
            {
                return NotFound("Warehouse doesn't exist");
            }

            return Ok(warehouse);
        }

        [HttpGet("Warehouses")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetWarehouses()
        {
            return Ok(_context.Warehouses.ToArray());
        }

        [HttpPost]
        [SecuredRight(UserRightType.Admin_Add)]
        public IActionResult AddWarehouse(CreateWarehouseDTO warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Warehouse dbWarehouse = warehouse.CreateWarehouse();

            _context.Warehouses.Add(dbWarehouse);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWarehouseByID), new { id = dbWarehouse.WarehouseID }, dbWarehouse);
        }

        [HttpPut("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult EditWarehouse(int id, EditWarehouseDTO editWarehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Warehouse warehouse = _context.Warehouses.Find(id);

            if (warehouse == null)
            {
                return NotFound("Warehouse doesn't exist");
            }

            warehouse.City = editWarehouse.City;
            warehouse.Country = editWarehouse.Country;
            warehouse.PostCode = editWarehouse.PostCode;
            warehouse.Street = editWarehouse.Street;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult DeleteWarehouse(int id)
        {
            Warehouse warehouse = _context.Warehouses.Find(id);

            if (warehouse == null)
            {
                return NotFound("Warehouse doesn't exist");
            }

            _context.Remove(warehouse);
            _context.SaveChanges();

            return NoContent();
        }
    }
}