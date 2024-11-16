using DrinkStorage.Application.Products.Responses;
using DrinkStorage.Persistence;
using DrinkStorage.Persistence.Products;
using Microsoft.Extensions.Logging;

namespace DrinkStorage.Application.Products;

public class ProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly ProductRepository _repository;
    private readonly ProductXlsxService _xlsxService;
    private IUnitOfWork _unitOfWork;

    public ProductService(
        ILogger<ProductService> logger,
        ProductRepository repository,
        ProductXlsxService xlsxService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _xlsxService = xlsxService;
        _unitOfWork = unitOfWork;
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

    public async Task ImportFromXlsx(Stream steam, CancellationToken token)
    {
        _logger.LogInformation("Import begin");
        var products = _xlsxService.ParseFromXlsx(steam);
        const int batchSize = 1000;
        var productBatch = new List<Product>(batchSize);
        int totalImported = 0;
        foreach (Product product in products)
        {
            if (productBatch.Count == batchSize)
            {
                int saved = await SaveProducts(productBatch, token);
                totalImported += saved;
                productBatch.Clear();
            }
            productBatch.Add(product);
        }
        if (productBatch.Count > 0)
        {
            await SaveProducts(productBatch, token);
        }
        _logger.LogInformation("Import completed, products saved: {}", totalImported);
    }

    private async Task<int> SaveProducts(List<Product> products, CancellationToken token)
    {
        try
        {
            await _repository.AddRangeAsync(products, token);
            int saved = await _unitOfWork.SaveChangesAsync(token);
            _logger.LogInformation("Import progress, imported in batch: {}", saved);
            return saved;
        }
        catch (Exception ex)
        {
            _logger.LogError("Import error: {}", ex);
            throw;
        }
    }
}
