using DrinkStorage.Application.Products;
using DrinkStorage.Persistence.Products;
using DrinkStorage.WebApi.Products.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DrinkStorage.WebApi.Products;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var info = await _productService.GetProductInfoAsync(token);
        var products = info.Products.Select(MapProduct)
            .ToList();
        return Ok(new ProductInfoResponse(products, info.MaxPrice));
    }

    [HttpGet("by-ids")]
    public async Task<IActionResult> GetAllByIds([FromQuery] List<Guid> ids, CancellationToken token)
    {
        var products = await _productService.GetByIdsAsync(ids, token);
        return Ok(products.Select(MapProduct).ToList());
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByBrandAndPrice(
        [FromQuery] int maxPrice,
        [FromQuery] Guid? brandId,
        CancellationToken token)
    {
        var products = await _productService.FindByBrandAndPrice(brandId, maxPrice, token);
        return Ok(products.Select(MapProduct).ToList());
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken token)
    {
        try
        {
            await _productService.ImportFromXlsx(file.OpenReadStream(), token);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static ProductResponse MapProduct(Product product) =>
        new(
            product.Id,
            product.Name,
            product.Price,
            product.Quantity,
            product.ImageUrl,
            product.BrandId.ToString());
}
