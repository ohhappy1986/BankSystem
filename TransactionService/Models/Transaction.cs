using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models
{
    /// <summary>
    /// Data Model for Transactions table
    /// </summary>
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public bool TransactionType { get; set; } //false is withdraw, true is deposit
        public decimal TransactionAmount { get; set; }
    }
}
