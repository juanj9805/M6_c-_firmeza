using System.ComponentModel.DataAnnotations;

namespace Firmeza.Domain.Models.Entities;

public class SaleDetail
{
    public int Id { get; set; }
    [Required]
    public int SaleId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }
    [Required]
    public decimal Total { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Sale? Sale { get; set; }
    public Product? Product { get; set; }
}