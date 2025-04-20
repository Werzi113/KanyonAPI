using System.ComponentModel.DataAnnotations;
using WebApplication1.Enums;

namespace WebApplication1.Models.DTO.UserRights
{
    public class RightDTO
    {
        [Required()]
        public UserRightType Right { get; set; }
    }
}