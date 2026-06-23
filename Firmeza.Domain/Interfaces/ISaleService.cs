using Firmeza.Domain.Models.Dtos;

namespace Firmeza.Domain.Interfaces;

public interface ISaleService
{
    Task<List<SaleDto>> GetAllSalesAsync();
    Task<SaleDto?> GetSaleByIdAsync(int id);
    Task<SaleDto> CreateSaleAsync(CreateSaleDto sale);
    Task<SaleDto?> UpdateSaleAsync(int id, CreateSaleDto sale);
    Task<bool> DeleteSaleAsync(int id);
}
