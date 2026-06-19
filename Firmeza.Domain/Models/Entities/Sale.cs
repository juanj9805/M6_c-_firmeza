using System.ComponentModel.DataAnnotations;

namespace Firmeza.Domain.Models.Entities;

public class Sale
{
    public int Id { get; set; }
    
    
    [Required]    
    public decimal SubTotal { get; set; }
    [Required]    
    public decimal Tax { get; set; }
    [Required]    
    public decimal Total { get; set; }
    [Required]    
    public int ClientId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Client? Client { get; set; }
    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

}