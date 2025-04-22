using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace WebApplication1.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public string PaymentMethod { get; set; }
        public bool Status { get; set; }
        public string? TransactionGatewayID {  get; set; }
        public int OrderID { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string PhoneNumber {  get; set; }
        public string Street { get; set; }
        public decimal ShippingFee { get; set; }
    }
}
