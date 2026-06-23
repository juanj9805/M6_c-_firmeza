using Firmeza.Domain.Data;
using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Domain.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        this._context = context;
    }
    
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        var found = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (found is null) return null;

        found.Name = product.Name;
        found.Description = product.Description;
        found.Stock = product.Stock;
        found.Type = product.Type;
        found.Price = product.Price;
        found.SKU = product.SKU;
        found.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return found;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var found = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (found is null)
        {
            return false;
        }

        _context.Products.Remove(found);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Products.CountAsync();
    }
}