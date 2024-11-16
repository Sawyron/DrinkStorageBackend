namespace DrinkStorage.Application.Orders.Exceptions;
public class ProductQuantityIsInsufficientException : Exception
{
    public ProductQuantityIsInsufficientException(Guid id, int quantity)
        : base($"Can't sell {quantity} of product with id {id}")
    {

    }
}
