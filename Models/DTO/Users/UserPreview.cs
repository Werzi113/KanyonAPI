namespace WebApplication1.Models.DTO.Users
{
    public class UserPreview
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public UserPreview(User user)
        {
            UserID = user.UserID;
            Username = user.Username;
            Email = user.Email;
        }
    }
}
