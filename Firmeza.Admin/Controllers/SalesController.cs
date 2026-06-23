using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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

    [HttpGet]
    public async Task<IActionResult> Receipt(int id)
    {
        var sale = await _service.GetSaleByIdAsync(id);
        if (sale is null) return NotFound();

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(t => t.FontSize(11).FontFamily("Arial"));

                page.Content().Column(col =>
                {
                    col.Spacing(16);

                    // Header
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("FIRMEZA").Bold().FontSize(24);
                            c.Item().Text("Construction Materials").FontSize(12).FontColor("#888888");
                        });
                        row.ConstantItem(160).Column(c =>
                        {
                            c.Item().Text($"Receipt #{sale.Id}").Bold().FontSize(14).AlignRight();
                            c.Item().Text(sale.CreatedAt.ToString("yyyy-MM-dd")).FontColor("#888888").AlignRight();
                        });
                    });

                    col.Item().LineHorizontal(1).LineColor("#333333");

                    // Client info
                    col.Item().Column(c =>
                    {
                        c.Item().Text("BILLED TO").Bold().FontSize(10).FontColor("#888888");
                        c.Item().Text(sale.ClientName).Bold().FontSize(13);
                    });

                    // Totals table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn();
                            cols.ConstantColumn(120);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background("#1a1a1a").Padding(8)
                                .Text("Description").Bold().FontColor("#ffffff");
                            header.Cell().Background("#1a1a1a").Padding(8)
                                .Text("Amount").Bold().FontColor("#ffffff").AlignRight();
                        });

                        table.Cell().BorderBottom(1).BorderColor("#333333").Padding(8).Text("Subtotal");
                        table.Cell().BorderBottom(1).BorderColor("#333333").Padding(8)
                            .Text($"${sale.SubTotal:N2}").AlignRight();

                        table.Cell().BorderBottom(1).BorderColor("#333333").Padding(8).Text("Tax");
                        table.Cell().BorderBottom(1).BorderColor("#333333").Padding(8)
                            .Text($"${sale.Tax:N2}").AlignRight();

                        table.Cell().Padding(8).Text("Total").Bold();
                        table.Cell().Padding(8).Text($"${sale.Total:N2}").Bold().AlignRight();
                    });

                    // Status
                    col.Item().AlignRight()
                        .Text($"Status: {sale.Status}").FontColor("#888888").FontSize(10);
                });

                page.Footer().AlignCenter()
                    .Text($"Generated {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC")
                    .FontSize(9).FontColor("#888888");
            });
        });

        var bytes = pdf.GeneratePdf();
        return File(bytes, "application/pdf", $"receipt-{sale.Id}.pdf");
    }
}
