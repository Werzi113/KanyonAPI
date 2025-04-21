using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.DiscountCodes
{
    public class CreateDiscountCodeDTO
    {
        [Required()]
        [StringLength(50)]
        public string Code { get; set; }

        [Required()]
        public DateTime ExpiryDate { get; set; }


        [Required()]
        [Range(0d,1d, ErrorMessage = "Invalid discount value")]
        public double Discount {  get; set; }

        public DiscountCode CreateDiscountCode()
        {
            DiscountCode discountCode = new DiscountCode();

            discountCode.CodeID = 0;
            discountCode.Code = Code;
            discountCode.ExpiryDate = ExpiryDate;
            discountCode.Discount = Convert.ToDecimal(Discount);

            return discountCode;
        }
    }
}