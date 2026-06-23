using System.ComponentModel.DataAnnotations;

namespace Firmeza.Domain.Models.Dtos;

public class CreateSaleDto
{
    [Required]
    public int ClientId { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [Required, Range(0.01, double.MaxValue)]
    public decimal SubTotal { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Tax { get; set; }

    [Required, Range(0.01, double.MaxValue)]
    public decimal Total { get; set; }
}
