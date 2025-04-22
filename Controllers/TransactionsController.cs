using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Transactions;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private MyContext context = new MyContext();
        public TransactionsController()
        {
            StripeConfiguration.ApiKey = "sk_test_51REBLzR9lMfje77miSJ4btyT1RSnoWf1bgCboXS0V80ggWg1OV3SB1O0c3PyiuEPBzZVCq8IpiqQMD6A9ocmh3ms00xhTKxR2V";
        }

        [HttpPost("CreatePaymentIntent")]
        public IActionResult CreatePaymentIntent(int amount)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount, 
                Currency = "usd",
                PaymentMethod = "pm_card_visa", 
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never"
                },
            };
            var service = new PaymentIntentService();
            PaymentIntent intent = service.Create(options);
            

            return Ok(new { clientSecret = intent.ClientSecret, ID = intent.Id });
        }
        [HttpPost("CreateTransaction")]
        public IActionResult CreateTransaction(TransactionDTO tran)
        {
            this.context.Transactions.Add(new Transaction()
            {
                FirstName = tran.FirstName,
                LastName = tran.LastName,
                PhoneNumber = tran.PhoneNumber,
                City = tran.City,
                Street = tran.Street,
                PostCode = tran.PostCode,
                Country = tran.Country,
                TransactionGatewayID = "Stripe",
                OrderID = tran.OrderID,
                PaymentMethod = tran.PaymentMethod,
                Status = false,
                ShippingFee = tran.ShippingFee,
            });
            this.context.SaveChanges();

            return Ok(tran);
        }

        [HttpGet("ConfirmPaymentIntent")]
        public IActionResult ConfirmPaymentIntent(string paymentIntentID)
        {
            var service = new PaymentIntentService();

            var options = new PaymentIntentConfirmOptions
            {
                PaymentMethod = "pm_card_visa"
            };

            var paymentIntent = service.Confirm(paymentIntentID, options);
            return Ok(paymentIntent);
        }
    }
}
