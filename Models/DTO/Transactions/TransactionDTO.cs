namespace WebApplication1.Models.DTO.Transactions
{
    public class TransactionDTO
    {
        public string PaymentMethod { get; set; }
        public int OrderID { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public decimal ShippingFee { get; set; }
    }
}
