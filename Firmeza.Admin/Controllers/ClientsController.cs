using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Admin.Controllers;

public class ClientsController : Controller
{
    private readonly IClientService _service;

    public ClientsController(IClientService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _service.GetAllClientsAsync();
        return View(clients);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var client = await _service.GetClientByIdAsync(id);
        if (client is null) return NotFound();
        return View(client);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateClientDto client)
    {
        if (!ModelState.IsValid) return View(client);
        await _service.CreateClientAsync(client);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = await _service.GetClientByIdAsync(id);
        if (client is null) return NotFound();

        var dto = new CreateClientDto
        {
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address
        };

        ViewData["Id"] = id;
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CreateClientDto client)
    {
        if (!ModelState.IsValid) return View(client);

        var updated = await _service.UpdateClientAsync(id, client);
        if (updated is null) return NotFound();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteClientAsync(id);
        return RedirectToAction("Index");
    }
}
