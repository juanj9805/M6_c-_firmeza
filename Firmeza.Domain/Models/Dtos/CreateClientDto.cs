using System.ComponentModel.DataAnnotations;

namespace Firmeza.Domain.Models.Dtos;

public class CreateClientDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;
}
