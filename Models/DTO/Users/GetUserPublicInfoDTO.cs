using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.Users
{
    public class GetUserPublicInfoDTO
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? City { get; set; }

        public string? Street { get; set; }

        public string? Country { get; set; }

        public string? PostCode { get; set; }

        public static GetUserPublicInfoDTO FromUser(User user)
        {
            GetUserPublicInfoDTO info = new GetUserPublicInfoDTO();

            info.UserID = user.UserID;
            info.Username = user.Username;
            info.Email = user.Email;
            info.CreatedAt = user.CreatedAt;
            info.FirstName = user.FirstName;
            info.LastName = user.LastName;
            info.PhoneNumber = user.PhoneNumber;
            info.City = user.City;
            info.Street = user.Street;
            info.Country = user.Country;
            info.PostCode = user.PostCode;

            return info;
        }
    }
}