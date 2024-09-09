using DrinkStorage.Application.Products.Responses;
using DrinkStorage.Persistence.Products;

namespace DrinkStorage.Application.Products;

public class ProductSerivce
{
    private readonly ProductRepository _repository;

    public ProductSerivce(ProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductInfo> GetProductInfoAsync(CancellationToken token)
    {
        var products = await _repository.FindAllAsync(token);
        return new ProductInfo(
            products,
            products.Select(p => p.Price).DefaultIfEmpty(0).Max());
    }

    public Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken token) =>
        _repository.FindByIdsAsync(ids.ToList(), token);

    public Task<List<Product>> FindByBrandAndPrice(Guid? brandId, int maxPrice, CancellationToken token) =>
        _repository.FindByBrandAndPrice(brandId, maxPrice, token);
}
