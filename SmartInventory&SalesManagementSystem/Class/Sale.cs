using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory_SalesManagementSystem
{
    [Table("Sales")]
    public class Sale
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
        }

        [Key]
        public int SaleId { get; set; }

        public DateTime? SaleDate { get; set; }

        // ===== Foreign Key =====
        [Required]
        public int UserId { get; set; }

        [Column(TypeName = "REAL")] // SQLite-friendly
        public decimal TotalAmount { get; set; }

        // ===== Navigation Properties =====
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
        public virtual User User { get; set; }
    }
}
