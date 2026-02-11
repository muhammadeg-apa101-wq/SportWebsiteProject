using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportEcommerce.API.Models.Entities
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAtAdd { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}