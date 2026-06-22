using System.ComponentModel.DataAnnotations;

namespace Firmeza.Domain.Models.Dtos;

public class CreateProductDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    [Required]
    public int Stock { get; set; }
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    [Required]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;
    
}