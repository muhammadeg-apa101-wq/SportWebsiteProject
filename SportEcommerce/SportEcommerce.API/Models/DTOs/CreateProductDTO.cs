using System.ComponentModel.DataAnnotations;

namespace SportEcommerce.API.Models.DTOs
{
    public class CreateProductDTO
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public string? Brand { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public List<string>? ImageGallery { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsFeatured { get; set; }
    }
}