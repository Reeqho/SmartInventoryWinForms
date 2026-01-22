using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory_SalesManagementSystem
{
    [Table("SaleDetails")]
    public class SaleDetail
    {
        [Key]
        public int SaleDetailId { get; set; }

        // ===== Foreign Keys =====
        [Required]
        public int SaleId { get; set; }

        [Required]
        public int ProductId { get; set; }

        // ===== Data =====
        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "REAL")] // SQLite-friendly
        public decimal Price { get; set; }

        [Column(TypeName = "REAL")]
        public decimal? SubTotal { get; set; }

        // ===== Navigation Properties =====
        public virtual Product Product { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
