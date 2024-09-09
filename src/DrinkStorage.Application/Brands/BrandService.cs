using DrinkStorage.Persistence.Brands;

namespace DrinkStorage.Application.Brands;

public class BrandService
{
    private readonly BrandRepository _repository;

    public BrandService(BrandRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Brand>> FindAllAsync(CancellationToken token) =>
        _repository.FindAllAsync(token);
}
