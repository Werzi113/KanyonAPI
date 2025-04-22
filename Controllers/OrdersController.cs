using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Orders;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private MyContext context = new MyContext();
        [HttpPost("Create")]
        public IActionResult CreateOrder(OrderCreateDTO order)
        {
            Order o = new Order()
            {
                OrderID = order.OrderID,
                OrderedAt = DateTime.Now,
                DiscountCodeID = order.DiscountCodeID,
                UserID = order.UserID,
            };

            this.context.Orders.Add(o);
            this.context.SaveChanges();

            foreach (var item in order.Details)
            {
                Product? p = this.context.Products.Where(x => x.ProductID == item.ProductID).First();

                if (p == null)
                {
                    return BadRequest($"Product with id {p.ProductID} doesn't exist");
                }

                this.context.OrderDetails.Add(new OrderDetail()
                {
                    Amount = item.Amount,
                    Discount = item.Discount,
                    OrderID = o.OrderID,
                    Price = p.Price * item.Amount,
                    ProductID = item.ProductID,
                });
            }
            this.context.SaveChanges();

            return Ok(o);
        }
    }
}
