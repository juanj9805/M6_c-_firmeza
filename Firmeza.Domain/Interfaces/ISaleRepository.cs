using Firmeza.Domain.Models.Entities;

namespace Firmeza.Domain.Interfaces;

public interface ISaleRepository
{
    Task<List<Sale>> GetAllSalesAsync();
    Task<Sale?> GetSaleByIdAsync(int id);
    Task<Sale> CreateSaleAsync(Sale sale);
    Task<Sale?> UpdateSaleAsync(int id, Sale sale);
    Task<bool> DeleteSaleAsync(int id);
    Task<int> GetCountAsync();
}
