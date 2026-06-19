using Firmeza.Domain.Data;
using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Domain.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> GetAllClientsAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> UpdateClientAsync(int id, Client client)
    {
        var found = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

        if (found is null) return null;

        _context.Entry(found).CurrentValues.SetValues(client);
        found.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return found;
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var found = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

        if (found is null) return false;

        _context.Clients.Remove(found);
        await _context.SaveChangesAsync();
        return true;
    }
}
