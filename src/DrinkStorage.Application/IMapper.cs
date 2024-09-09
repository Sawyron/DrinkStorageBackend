namespace DrinkStorage.Application;

public interface IMapper<TInput, TOutput>
{
    TOutput Map(TInput input);
}
