using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Enums;
using WebApplication1.Models.DTO.Stocks;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {

        private MyContext _context = new MyContext();

        [HttpGet("Stocks")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetWarehouses()
        {
            return Ok(_context.Stocks.ToArray());
        }

        [HttpGet("Warehouse/{id}")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetStocksByWarehouse(int id)
        {
            if (!_context.Warehouses.Any(Warehouse => Warehouse.WarehouseId == id))
            {
                return NotFound("Warehouse doesn't exist");   
            }

            return Ok(_context.Stocks.Where(Stock => Stock.WarehouseID == id).ToArray());
        }

        [HttpGet("Product/{id}")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetStocksByProduct(int id)
        {
            if (!_context.Products.Any(Product => Product.ProductID == id))
            {
                return NotFound("Product doesn't exist");
            }

            return Ok(_context.Stocks.Where(Stock => Stock.ProductID == id).ToArray());
        }

        [HttpGet("{id}")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetStockByID(int id)
        {
            Stock stock = _context.Stocks.Find(id);

            if (stock == null)
            {
                return NotFound("Stock doesn't exist");
            }

            return Ok(stock);
        }

        [HttpGet("Amount/{id}")]
        public IActionResult GetStockedAmountOfProduct(int id)
        {
            if (!_context.Products.Any(Product => Product.ProductID == id))
            {
                return NotFound("Product doesn't exist");
            }
            if (!_context.Stocks.Any(Stock => Stock.ProductID == id))
            {
                return NotFound("Product doesn't have a stock history");
            }

            StockAmountDTO stockAmountDTO = new StockAmountDTO();

            stockAmountDTO.ProductID = id;
            stockAmountDTO.Amount = _context.Stocks.Where(Stock => Stock.ProductID == id).Sum(Stock => Stock.Amount);

            return Ok(stockAmountDTO);
        }

        [HttpGet("AmountWarehouses/{id}")]
        public IActionResult GetStockedAmountOfProductByWarehouses(int id)
        {
            if (!_context.Products.Any(Product => Product.ProductID == id))
            {
                return NotFound("Product doesn't exist");
            }
            if (!_context.Stocks.Any(Stock => Stock.ProductID == id))
            {
                return NotFound("Product doesn't have a stock history");
            }

            List<StockAmountWarehouseDTO> stocks = new List<StockAmountWarehouseDTO>();

            var query = _context.Stocks.Where(Stock => Stock.ProductID == id).GroupBy(Stock => Stock.WarehouseID);

            foreach (var stockGroup in query)
            {
                stocks.Add(new StockAmountWarehouseDTO() { ProductID = id, WarehouseID = stockGroup.Key, Amount = stockGroup.Sum(Stock => Stock.Amount) });
            }

            return Ok(stocks);
        }

        [HttpPost]
        [SecuredRight(UserRightType.Admin_Add)]
        public IActionResult AddStock(CreateStockDTO stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Products.Any(Product => Product.ProductID == stock.ProductID))
            {
                return NotFound("Product doesn't exist");
            }
            if (!_context.Warehouses.Any(Warehouse => Warehouse.WarehouseId == stock.WarehouseID))
            {
                return NotFound("Warehouse doesn't exist");
            }

            Stock dbStock = stock.CreateStock();
            _context.Stocks.Add(dbStock);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStockByID), new { id = dbStock.StockID }, dbStock);
        }

        [HttpPut("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult EditStock(int id, EditStockDTO stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Stock dbStock = _context.Stocks.Find(id);

            if (dbStock == null)
            {
                return NotFound("Stock doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == stock.ProductID))
            {
                return NotFound("Product doesn't exist");
            }
            if (!_context.Warehouses.Any(Warehouse => Warehouse.WarehouseId == stock.WarehouseID))
            {
                return NotFound("Warehouse doesn't exist");
            }

            dbStock.ProductID = stock.ProductID;
            dbStock.Amount = stock.Amount;
            dbStock.WarehouseID = stock.WarehouseID;
            dbStock.Date = stock.Date;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult DeleteStock(int id)
        {
            Stock stock = _context.Stocks.Find(id);

            if (stock == null)
            {
                return NotFound("Stock doesn't exist");
            }

            _context.Stocks.Remove(stock);
            _context.SaveChanges();

            return NoContent();
        }
    }
}