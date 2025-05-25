namespace WebApplication1.Models.DTO.Orders
{
    public class OrderCreateDTO
    {
        public int? DiscountCodeID { get; set; }
        public int? UserID { get; set; }
        public OrderDetailDTO[] Details { get; set; }



    }
}
