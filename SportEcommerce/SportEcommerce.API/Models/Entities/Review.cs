using System.ComponentModel.DataAnnotations;

namespace SportEcommerce.API.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsApproved { get; set; } = false;
    }
}