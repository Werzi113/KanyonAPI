using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.DiscountCodes
{
    public class EditDiscountCodeDTO
    {
        [Required()]
        [StringLength(50)]
        public string Code { get; set; }

        [Required()]
        public DateTime ExpiryDate { get; set; }


        [Required()]
        [Range(0,1)]
        public decimal Discount {  get; set; }
    }
}