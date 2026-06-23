using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Admin.Controllers;

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
}