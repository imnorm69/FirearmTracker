using FirearmTracker.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirearmTracker.Core.Models
{
    public class AmmunitionTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AmmunitionId { get; set; }

        [ForeignKey("AmmunitionId")]
        public virtual Ammunition Ammunition { get; set; } = null!;

        [Required]
        public AmmunitionTransactionType TransactionType { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        // For Purchase transactions
        public decimal? PurchasePrice { get; set; }

        // For Sale transactions
        public decimal? SalePrice { get; set; }

        public string? BuyerName { get; set; }

        // For Consumption via Range Session
        public int? ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public virtual Activity? Activity { get; set; }

        public string Notes { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}