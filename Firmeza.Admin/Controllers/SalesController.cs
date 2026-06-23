using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Admin.Controllers;

[Authorize(Roles = "Admin")]
public class SalesController : Controller
{
    private readonly ISaleService _service;
    private readonly ILogger<SalesController> _logger;

    public SalesController(ISaleService service, ILogger<SalesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var sales = await _service.GetAllSalesAsync();
        return View(sales);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var sale = await _service.GetSaleByIdAsync(id);
        if (sale is null) return NotFound();
        return View(sale);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleDto sale)
    {
        if (!ModelState.IsValid) return View(sale);

        try
        {
            await _service.CreateSaleAsync(sale);
            TempData["Success"] = "Sale created successfully.";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create sale");
            ModelState.AddModelError("", "DB unavailable");
            return View(sale);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var sale = await _service.GetSaleByIdAsync(id);
        if (sale is null) return NotFound();

        var dto = new CreateSaleDto
        {
            ClientId = sale.ClientId,
            SubTotal = sale.SubTotal,
            Tax = sale.Tax,
            Total = sale.Total,
            Status = sale.Status
        };

        ViewData["Id"] = id;
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CreateSaleDto sale)
    {
        if (!ModelState.IsValid) return View(sale);

        try
        {
            var updated = await _service.UpdateSaleAsync(id, sale);
            if (updated is null) return NotFound();

            TempData["Success"] = "Sale updated successfully.";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update sale");
            ModelState.AddModelError("", "DB unavailable");
            return View(sale);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteSaleAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete sale");
            TempData["Error"] = "Could not delete sale. Please try again.";
            return RedirectToAction("Index");
        }
    }
}
