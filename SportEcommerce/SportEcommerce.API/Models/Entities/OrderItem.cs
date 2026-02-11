using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportEcommerce.API.Models.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAtPurchase { get; set; }
    }
}