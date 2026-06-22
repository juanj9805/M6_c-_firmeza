using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Admin.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
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
        var updateProduct = await _service.UpdateProductAsync(id, product);

        if (updateProduct is null)
        {
            return NotFound();
        }
        
        return RedirectToAction("Index");
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

        await _service.CreateProductAsync(product);

        return RedirectToAction("Index");
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteProductAsync(id);

        return RedirectToAction("Index");
    }
}