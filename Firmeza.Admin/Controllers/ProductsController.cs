using Firmeza.Domain.Interfaces;
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
        var products = await _service.GetAllProductsAsync();
        return View(products);
    }
}