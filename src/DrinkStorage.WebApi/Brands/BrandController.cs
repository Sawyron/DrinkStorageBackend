using DrinkStorage.Application.Brands;
using DrinkStorage.Persistence.Brands;
using DrinkStorage.WebApi.Brands.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DrinkStorage.WebApi.Brands;

[Route("api/v1/[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly BrandService _service;

    public BrandController(BrandService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var brands = await _service.FindAllAsync(token);
        return Ok(brands.Select(MapBrand).ToList());
    }

    private static BrandResponse MapBrand(Brand brand) =>
        new(brand.Id, brand.Name);
}
