using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessApplication.Model
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string InvoiceNumber { get; set; }
        public DateTime DueDate { get; set; }
        public required Address BillingAddress { get; set; }
        public double Discount { get; set; }
        public double TaxPercentage { get; set; }
        public required string PaymentMethod { get; set; } // Use PaymentInformationConstants for consistency
        public required string PaymentStatus { get; set; } // Use PaymentInformationConstants for consistency
        public required Order OrderInformations { get; set; }
    }
}