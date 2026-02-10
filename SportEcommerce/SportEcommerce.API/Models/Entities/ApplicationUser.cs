using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportEcommerce.API.Models.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual Cart? Cart { get; set; }

        // Full Name helper
        public string FullName => $"{FirstName} {LastName}";
    }
}