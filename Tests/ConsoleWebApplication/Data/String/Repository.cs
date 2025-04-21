namespace ConsoleWebApplication.Data.String;

public interface IRepository<T> : IReadonlyRepository<T, string> where T : class, IEntity<string>;

public abstract class EfCoreRepository<T> : EfCoreIdentifierRepository<T, string>, IRepository<T> where T : class, IEntity<string>;