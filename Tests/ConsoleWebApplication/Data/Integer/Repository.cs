namespace ConsoleWebApplication.Data.Integer;

public interface IRepository<T> : IReadonlyRepository<T, int> where T : class, IEntity<int>;

public abstract class EfCoreRepository<T> : EfCoreIdentifierRepository<T, int>, IRepository<T> where T : class, IEntity<int>;