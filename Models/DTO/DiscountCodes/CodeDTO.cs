using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.DiscountCodes
{
    public class CodeDTO
    {
        [Required()]
        [StringLength(50)]
        public string Code { get; set; }
    }
}