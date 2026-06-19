using System.ComponentModel.DataAnnotations;

namespace Firmeza.Domain.Models.Entities;

public class Client
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;
    
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();        

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}