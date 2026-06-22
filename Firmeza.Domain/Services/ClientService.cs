using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Firmeza.Domain.Models.Entities;

namespace Firmeza.Domain.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ClientDto>> GetAllClientsAsync()
    {
        var clients = await _repository.GetAllClientsAsync();
        return clients.Select(c => new ClientDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Address = c.Address,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
    }

    public async Task<ClientDto?> GetClientByIdAsync(int id)
    {
        var client = await _repository.GetClientByIdAsync(id);
        if (client is null) return null;

        return new ClientDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address,
            IsActive = client.IsActive,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt
        };
    }

    public async Task<ClientDto> CreateClientAsync(CreateClientDto client)
    {
        var newClient = new Client
        {
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address
        };

        var saved = await _repository.CreateClientAsync(newClient);

        return new ClientDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Email = saved.Email,
            Phone = saved.Phone,
            Address = saved.Address,
            IsActive = saved.IsActive,
            CreatedAt = saved.CreatedAt,
            UpdatedAt = saved.UpdatedAt
        };
    }

    public async Task<ClientDto?> UpdateClientAsync(int id, CreateClientDto client)
    {
        var updateClient = new Client
        {
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address
        };

        var saved = await _repository.UpdateClientAsync(id, updateClient);
        if (saved is null) return null;

        return new ClientDto
        {
            Id = saved.Id,
            Name = saved.Name,
            Email = saved.Email,
            Phone = saved.Phone,
            Address = saved.Address,
            IsActive = saved.IsActive,
            CreatedAt = saved.CreatedAt,
            UpdatedAt = saved.UpdatedAt
        };
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        return await _repository.DeleteClientAsync(id);
    }
}
