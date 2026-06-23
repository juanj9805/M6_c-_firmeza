using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Firmeza.Admin.Controllers;

[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }
    public async Task<IActionResult> Index()
    {
        var productsDto = await _service.GetAllProductsAsync();
        return View(productsDto);
    }
    
    public async Task<IActionResult> Detail(int id)
    {
        var productDto = await _service.GetProductByIdAsync(id);
        if (productDto is null)
        {
            return NotFound();
        }
        return View(productDto);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        var dto = new CreateProductDto
        {
            Name = product.Name,
            Description = product.Description,
            Stock = product.Stock,
            Type = product.Type,
            Price = product.Price,
            SKU = product.SKU
        };
        
        ViewData["Id"] = id;
        
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CreateProductDto product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        try
        {
            var updateProduct = await _service.UpdateProductAsync(id, product);

            if (updateProduct is null)
            {
                return NotFound();
            }
            
            TempData["Success"] = "Product updated successfully.";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update product");
            ModelState.AddModelError("", "DB unavailable");
            return View(product);
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        try
        {
            await _service.CreateProductAsync(product);
            TempData["Success"] = "Product created successfully.";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create product");
            ModelState.AddModelError("", "DB unavailable");
           return View(product);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteProductAsync(id);

            return RedirectToAction("Index");

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete product");
            TempData["Error"] = "Could not delete product. Please try again.";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult Import() => View();

    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            ModelState.AddModelError("", "Please select an Excel file.");
            return View();
        }

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("", "Only .xlsx files are supported.");
            return View();
        }

        int created = 0;
        var errors = new List<string>();

        try
        {
            using var stream = file.OpenReadStream();
            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets[0];

            if (sheet.Dimension is null)
            {
                ModelState.AddModelError("", "The file is empty.");
                return View();
            }

            for (int row = 2; row <= sheet.Dimension.Rows; row++)
            {
                var name = sheet.Cells[row, 1].Value?.ToString()?.Trim();
                var sku = sheet.Cells[row, 2].Value?.ToString()?.Trim();
                var type = sheet.Cells[row, 3].Value?.ToString()?.Trim();
                var stockRaw = sheet.Cells[row, 4].Value?.ToString()?.Trim();
                var priceRaw = sheet.Cells[row, 5].Value?.ToString()?.Trim();
                var description = sheet.Cells[row, 6].Value?.ToString()?.Trim();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(sku) ||
                    string.IsNullOrEmpty(type) || string.IsNullOrEmpty(stockRaw) ||
                    string.IsNullOrEmpty(priceRaw))
                {
                    errors.Add($"Row {row}: missing required fields.");
                    continue;
                }

                if (!int.TryParse(stockRaw, out var stock) || !decimal.TryParse(priceRaw, out var price))
                {
                    errors.Add($"Row {row}: invalid Stock or Price format.");
                    continue;
                }

                await _service.CreateProductAsync(new CreateProductDto
                {
                    Name = name,
                    SKU = sku,
                    Type = type,
                    Stock = stock,
                    Price = price,
                    Description = description
                });

                created++;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to import products from Excel");
            ModelState.AddModelError("", "Failed to process the file.");
            return View();
        }

        TempData["Success"] = $"{created} product(s) imported successfully.";
        if (errors.Any())
            TempData["Error"] = string.Join(" | ", errors);

        return RedirectToAction("Index");
    }
}