namespace DrinkStorage.Application.Orders.Exceptions;

public class ProductsNotFoundException : Exception
{
    public ProductsNotFoundException(IEnumerable<Guid> ids) :
        base($"Products not found: {string.Join(", ", ids)}")
    {

    }
}
