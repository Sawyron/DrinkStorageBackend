namespace DrinkStorage.Application.Orders.Exceptions;
public class ProductQuantityIsUnsufficientExcettion : Exception
{
    public ProductQuantityIsUnsufficientExcettion(Guid id, int quantity)
        : base($"Can't sell {quantity} of product with id {id}")
    {

    }
}
