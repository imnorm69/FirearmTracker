using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirearmTracker.Core.Models
{
    public class FirearmAmmunitionLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FirearmId { get; set; }

        [ForeignKey("FirearmId")]
        public virtual Firearm Firearm { get; set; } = null!;

        [Required]
        public int AmmunitionId { get; set; }

        [ForeignKey("AmmunitionId")]
        public virtual Ammunition Ammunition { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        public string Notes { get; set; } = string.Empty;

        [Required]
        public bool IsDeleted { get; set; }
    }
}