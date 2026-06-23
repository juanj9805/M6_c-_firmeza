using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Firmeza.Domain.Models.Entities;

namespace Firmeza.Domain.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _repository;

    public SaleService(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SaleDto>> GetAllSalesAsync()
    {
        var response = await _repository.GetAllSalesAsync();

        return response.Select(s => new SaleDto
        {
            Id = s.Id,
            ClientId = s.ClientId,
            ClientName = s.Client?.Name ?? string.Empty,
            SubTotal = s.SubTotal,
            Tax = s.Tax,
            Total = s.Total,
            Status = s.Status,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
    }

    public async Task<SaleDto?> GetSaleByIdAsync(int id)
    {
        var response = await _repository.GetSaleByIdAsync(id);

        if (response is null) return null;

        return new SaleDto
        {
            Id = response.Id,
            ClientId = response.ClientId,
            ClientName = response.Client?.Name ?? string.Empty,
            SubTotal = response.SubTotal,
            Tax = response.Tax,
            Total = response.Total,
            Status = response.Status,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt
        };
    }

    public async Task<SaleDto> CreateSaleAsync(CreateSaleDto sale)
    {
        var newSale = new Sale
        {
            ClientId = sale.ClientId,
            SubTotal = sale.SubTotal,
            Tax = sale.Tax,
            Total = sale.Total,
            Status = sale.Status
        };

        var saved = await _repository.CreateSaleAsync(newSale);

        return new SaleDto
        {
            Id = saved.Id,
            ClientId = saved.ClientId,
            ClientName = saved.Client?.Name ?? string.Empty,
            SubTotal = saved.SubTotal,
            Tax = saved.Tax,
            Total = saved.Total,
            Status = saved.Status,
            CreatedAt = saved.CreatedAt,
            UpdatedAt = saved.UpdatedAt
        };
    }

    public async Task<SaleDto?> UpdateSaleAsync(int id, CreateSaleDto sale)
    {
        var updatedSale = new Sale
        {
            ClientId = sale.ClientId,
            SubTotal = sale.SubTotal,
            Tax = sale.Tax,
            Total = sale.Total,
            Status = sale.Status
        };

        var saved = await _repository.UpdateSaleAsync(id, updatedSale);

        if (saved is null) return null;

        return new SaleDto
        {
            Id = saved.Id,
            ClientId = saved.ClientId,
            ClientName = saved.Client?.Name ?? string.Empty,
            SubTotal = saved.SubTotal,
            Tax = saved.Tax,
            Total = saved.Total,
            Status = saved.Status,
            CreatedAt = saved.CreatedAt,
            UpdatedAt = saved.UpdatedAt
        };
    }

    public async Task<bool> DeleteSaleAsync(int id)
    {
        return await _repository.DeleteSaleAsync(id);
    }
}
