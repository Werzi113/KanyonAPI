using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.Users
{
    public class ChangePasswordDTO
    {
        [Required()]
        [StringLength(100)]
        [MinLength(5)]
        public string NewPassword { get; set; }

        [Required()]
        [StringLength(100)]
        [MinLength(5)]
        public string OldPassword { get; set; }
    }
}