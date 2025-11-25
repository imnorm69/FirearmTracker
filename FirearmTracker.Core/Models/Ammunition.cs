using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FirearmTracker.Core.Enums;

namespace FirearmTracker.Core.Models
{
    public class Ammunition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Caliber { get; set; } = string.Empty;

        public string? Manufacturer { get; set; }

        public string? BulletType { get; set; }

        public int? GrainWeight { get; set; }

        public string? LotNumber { get; set; }

        public string? StorageLocation { get; set; }

        public string Notes { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        // Navigation property
        public virtual ICollection<AmmunitionTransaction> Transactions { get; set; } = new List<AmmunitionTransaction>();

        // Navigation property for linked firearms
        public virtual ICollection<FirearmAmmunitionLink> FirearmLinks { get; set; } = new List<FirearmAmmunitionLink>();

        // Calculated property - current quantity based on transactions
        [NotMapped]
        public int CurrentQuantity
        {
            get
            {
                if (Transactions == null || !Transactions.Any())
                    return 0;

                return Transactions
                    .Where(t => !t.IsDeleted)
                    .Sum(t => t.TransactionType == AmmunitionTransactionType.Purchase
                        ? t.Quantity
                        : -t.Quantity);
            }
        }
    }
}