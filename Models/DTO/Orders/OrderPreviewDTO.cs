namespace WebApplication1.Models.DTO.Orders
{
    public class OrderPreviewDTO
    {
        public int OrderID { get; set; }

        public DateTime OrderedAt { get; set; }
        
        public bool Delivered { get; set; }

        public decimal Price { get; set; }

        public decimal CodeDiscount { get; set; }
    }
}
