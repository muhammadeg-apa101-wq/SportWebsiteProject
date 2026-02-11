using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportEcommerce.API.Models.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Pending, Processing, Shipped, Delivered, Cancelled

        [Required]
        public string ShippingAddress { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}