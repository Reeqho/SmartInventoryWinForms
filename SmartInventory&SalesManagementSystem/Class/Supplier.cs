using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory_SalesManagementSystem
{
    [Table("Suppliers")]
    public class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(150)]
        public string SupplierName { get; set; }

        [MaxLength(30)]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
