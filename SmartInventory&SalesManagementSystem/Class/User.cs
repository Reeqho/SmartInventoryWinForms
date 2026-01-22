using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory_SalesManagementSystem
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Sales = new HashSet<Sale>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        // ===== Foreign Key =====
        [Required]
        public int RoleId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        // ===== Navigation Properties =====
        public virtual Role Role { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
