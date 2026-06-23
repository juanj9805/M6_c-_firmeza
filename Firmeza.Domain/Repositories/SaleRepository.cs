using Firmeza.Domain.Data;
using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Domain.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sale>> GetAllSalesAsync()
    {
        return await _context.Sales.Include(s => s.Client).ToListAsync();
    }

    public async Task<Sale?> GetSaleByIdAsync(int id)
    {
        return await _context.Sales.Include(s => s.Client).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Sale> CreateSaleAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
        return sale;
    }

    public async Task<Sale?> UpdateSaleAsync(int id, Sale sale)
    {
        var found = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

        if (found is null) return null;

        found.ClientId = sale.ClientId;
        found.SubTotal = sale.SubTotal;
        found.Tax = sale.Tax;
        found.Total = sale.Total;
        found.Status = sale.Status;
        found.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return found;
    }

    public async Task<bool> DeleteSaleAsync(int id)
    {
        var found = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

        if (found is null) return false;

        _context.Sales.Remove(found);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Sales.CountAsync();
    }
}
