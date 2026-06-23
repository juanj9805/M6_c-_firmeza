using Firmeza.Domain.Models.Dtos;

namespace Firmeza.Domain.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> Dashboard();
}