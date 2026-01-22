using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory_SalesManagementSystem
{
    [Table("Products")]
    public class Product
    {
        public Product()
        {
            SaleDetails = new HashSet<SaleDetail>();
        }

        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; }

        // ===== Foreign Keys =====
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }

        // ===== Data =====
        public int Stock { get; set; }

        [Column(TypeName = "REAL")] // SQLite-friendly
        public decimal Price { get; set; }

        public DateTime? CreatedAt { get; set; }

        // ===== Navigation Properties =====
        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
