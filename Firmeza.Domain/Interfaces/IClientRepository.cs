using Firmeza.Domain.Models.Entities;

namespace Firmeza.Domain.Interfaces;

public interface IClientRepository
{
    Task<List<Client>> GetAllClientsAsync();
    Task<Client?> GetClientByIdAsync(int id);
    Task<Client> CreateClientAsync(Client client);
    Task<Client?> UpdateClientAsync(int id, Client client);
    Task<bool> DeleteClientAsync(int id);
}
