using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;

namespace Firmeza.Domain.Services;

public class DashboardService : IDashboardService
{
    private readonly IProductRepository _productRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ISaleRepository _saleRepository;

    public DashboardService(IProductRepository productRepository, IClientRepository clientRepository, ISaleRepository saleRepository)
    {
        _productRepository = productRepository;
        _clientRepository = clientRepository;
        _saleRepository = saleRepository;
    }

    public async Task<DashboardDto> Dashboard()
    {
        var productCount = await _productRepository.GetCountAsync();
        var clientCount = await _clientRepository.GetCountAsync();
        var saleCount = await _saleRepository.GetCountAsync();

        return new DashboardDto
        {
            ClientTotalAmount = clientCount,
            ProductTotalAmount = productCount,
            SaleTotalAmount = saleCount
        };
    }
}