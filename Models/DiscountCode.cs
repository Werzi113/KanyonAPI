namespace WebApplication1.Models
{
    public class DiscountCode
    {
        public int CodeID { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Discount { get; set; }

    }
}
