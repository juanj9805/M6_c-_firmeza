using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Entities;

namespace Firmeza.Domain.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Client>> GetAllClientsAsync()
    {
        return await _repository.GetAllClientsAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _repository.GetClientByIdAsync(id);
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        return await _repository.CreateClientAsync(client);
    }

    public async Task<Client?> UpdateClientAsync(int id, Client client)
    {
        return await _repository.UpdateClientAsync(id, client);
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        return await _repository.DeleteClientAsync(id);
    }
}
