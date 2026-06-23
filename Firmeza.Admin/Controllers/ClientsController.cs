using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Admin.Controllers;

public class ClientsController : Controller
{
    private readonly IClientService _service;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IClientService service, ILogger<ClientsController> logger)
    {
        _service = service;
        _logger = logger;
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

        try
        {
            await _service.CreateClientAsync(client);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create client");
            ModelState.AddModelError("", "DB unavailable");
            return View(client);
        }
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

        try
        {
            var updated = await _service.UpdateClientAsync(id, client);
            if (updated is null) return NotFound();

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to edit client");
            ModelState.AddModelError("", "DB unavailable");
            return View(client);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteClientAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete client");
            TempData["Error"] = "Could not delete client. Please try again.";
            return RedirectToAction("Index");
        }
    }
}
