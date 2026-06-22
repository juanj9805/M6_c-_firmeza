using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Firmeza.Domain.Models.Entities;

namespace Firmeza.Domain.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var response = await _repository.GetAllProductsAsync();

        return response.Select(r => new ProductDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            Stock = r.Stock,
            Type = r.Type,
            Price = r.Price,
            SKU = r.SKU,
            IsActive = r.IsActive,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,

        }).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var response =  await _repository.GetProductByIdAsync(id);

        if (response is null)
        {
            return null;
        }
        
        return new ProductDto
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            Stock = response.Stock,
            Type = response.Type,
            Price = response.Price,
            SKU = response.SKU,
            IsActive = response.IsActive,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt,

        };
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto product)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Stock = product.Stock,
            Type = product.Type,
            Price = product.Price,
            SKU = product.SKU,
    
        };

        var saved = await _repository.CreateProductAsync(newProduct);

        var result = new ProductDto()
        {
            Id = saved.Id,
            Name = saved.Name,
            Description = saved.Description,
            Stock = saved.Stock,
            Type = saved.Type,
            Price = saved.Price,
            SKU = saved.SKU,
            IsActive = saved.IsActive,
            CreatedAt = saved.CreatedAt,
            UpdatedAt = saved.UpdatedAt,
        };

        return result;
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, CreateProductDto product)
    {

        var updateProduct = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Stock = product.Stock,
            Type = product.Type,
            Price = product.Price,
            SKU = product.SKU,
    
        };

        var saved = await _repository.UpdateProductAsync(id, updateProduct);

        if (saved is null)
        {
            return null;
        }

        var result = new ProductDto()
        {
            Id = saved.Id,
            Name = saved.Name,
            Description = saved.Description,
            Stock = saved.Stock,
            Type = saved.Type,
            Price = saved.Price,
            SKU = saved.SKU,
            IsActive = saved.IsActive,
            CreatedAt = saved.CreatedAt,
            UpdatedAt = saved.UpdatedAt,
        };

        return result;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _repository.DeleteProductAsync(id);
    }
}