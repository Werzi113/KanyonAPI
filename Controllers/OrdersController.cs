using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Orders;
using WebApplication1.Models.DTO.Wishlist;

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
                    Discount = p.Discount,
                    OrderID = o.OrderID,
                    UnitPrice = p.Price,
                    ProductID = item.ProductID,
                });
            }
            this.context.SaveChanges();

            return Ok(o);
        }

        [HttpGet("User/{id}")]
        [SecuredID]
        public IActionResult GetOrderPreviewsByUser(int id)
        {
            if (context.Users.Find(id) == null)
            {
                return NotFound("User does not exist");
            }

            List<OrderPreviewDTO> res = new List<OrderPreviewDTO>();
            
            foreach (var item in context.Orders.Where(order => order.UserID == id).ToArray())
            {
                OrderPreviewDTO orderPreviewDTO = new OrderPreviewDTO();

                orderPreviewDTO.Delivered = item.DeliveredAt != null;
                orderPreviewDTO.OrderID = item.OrderID;
                orderPreviewDTO.OrderedAt = item.OrderedAt;
                orderPreviewDTO.CodeDiscount = 0;
                decimal price = 0;

                foreach (var itemDetail in context.OrderDetails.Where(detail => detail.OrderID == item.OrderID).ToArray())
                {
                    price += itemDetail.UnitPrice * (1 - itemDetail.Discount) * itemDetail.Amount;
                }

                if (item.DiscountCodeID != null)
                {
                    DiscountCode discountCode = context.DiscountCodes.Find(item.DiscountCodeID);
                    if (discountCode != null)
                    {
                        price *= (1 - discountCode.Discount);
                        orderPreviewDTO.CodeDiscount = discountCode.Discount;
                    }
                }
                orderPreviewDTO.Price = price;
                res.Add(orderPreviewDTO);
            }

            return Ok(res);
        }

        [HttpGet("Products/{id}")]
        public IActionResult GetOrderProductsPreviews(int id)
        {
            if (context.Orders.Find(id) == null)
            {
                return NotFound("Order doesn't exist");
            }

            List<OrderProductDTO> products = new List<OrderProductDTO>();

            foreach (var item in context.OrderDetails.Where(product => product.OrderID == id).ToArray())
            {
                Product product = context.Products.Find(item.ProductID);

                if (product == null)
                {
                    continue;
                }

                OrderProductDTO productDTO = new OrderProductDTO()
                {
                    Discount = item.Discount,
                    ProductID = product.ProductID,
                    Price = item.UnitPrice,
                    Name = product.Name,
                    OrderID = id,
                    Quantity = item.Amount
                };

                var img = context.ProductPictures.FirstOrDefault(pic => pic.IsPreview == true && pic.ProductID == product.ProductID);

                if (img == null)
                {
                    productDTO.ImageURL = "";
                }
                else
                {
                    string baseUrl = $"{Request.Scheme}://{Request.Host}";
                    productDTO.ImageURL = $"{baseUrl}{img.PicturePath}";
                }

                products.Add(productDTO);
            }

            return Ok(products.ToArray());
        }
    }
}
