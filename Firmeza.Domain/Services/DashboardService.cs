using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;

namespace Firmeza.Domain.Services;

public class DashboardService : IDashboardService
{
    private readonly IProductRepository _productRepository;
    private readonly IClientRepository _clientRepository;

    public DashboardService(IProductRepository productRepository, IClientRepository clientRepository)
    {
        _productRepository = productRepository;
        _clientRepository = clientRepository;
    }

    public async Task<DashboardDto> Dashboard()
    {
        var productCount = await _productRepository.GetCountAsync();
        var clientCount = await _clientRepository.GetCountAsync();

        return new DashboardDto
        {
            ClientTotalAmount = clientCount,
            ProductTotalAmount = productCount
        };
    }
}