using System.ComponentModel.DataAnnotations;

namespace SportEcommerce.API.Models.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}