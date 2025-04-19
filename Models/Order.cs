namespace WebApplication1.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public int? DiscountCodeID { get; set; }
        public int? UserID { get; set; }


    }
}
