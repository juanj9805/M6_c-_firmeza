using Firmeza.Domain.Models.Dtos;

namespace Firmeza.Domain.Interfaces;

public interface IClientService
{
    Task<List<ClientDto>> GetAllClientsAsync();
    Task<ClientDto?> GetClientByIdAsync(int id);
    Task<ClientDto> CreateClientAsync(CreateClientDto client);
    Task<ClientDto?> UpdateClientAsync(int id, CreateClientDto client);
    Task<bool> DeleteClientAsync(int id);
}
