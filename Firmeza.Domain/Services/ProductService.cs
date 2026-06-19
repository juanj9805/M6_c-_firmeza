using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Entities;

namespace Firmeza.Domain.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _repository.GetAllProductsAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _repository.GetProductByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        return await _repository.CreateProductAsync(product);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        return await _repository.UpdateProductAsync(id, product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _repository.DeleteProductAsync(id);
    }
}