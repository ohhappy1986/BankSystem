using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    /// <summary>
    /// Data Model for Accounts table
    /// </summary>
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
