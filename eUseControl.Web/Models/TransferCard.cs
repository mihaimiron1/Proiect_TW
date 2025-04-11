using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class TransferCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BankSender { get; set; } = "Maib";

        [Required]
        public string CardSender { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public string CardBeneficiary { get; set; }

        [Required]
        public string BankBeneficiary { get; set; } = "Maib";

        public DateTime TransferDate { get; set; } = DateTime.Now;
    }
} 