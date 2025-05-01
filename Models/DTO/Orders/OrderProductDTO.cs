namespace WebApplication1.Models.DTO.Orders
{
    public class OrderProductDTO
    {
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public string ImageURL { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public string Name { get; set; }
    }
}
