using Firmeza.Domain.Models.Dtos;

namespace Firmeza.Domain.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto product);
    Task<ProductDto?> UpdateProductAsync(int id, CreateProductDto product);
    Task<bool> DeleteProductAsync(int id);
}