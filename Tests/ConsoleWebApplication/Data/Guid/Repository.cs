namespace ConsoleWebApplication.Data.Guid;

public interface IRepository<T> : IReadonlyRepository<T, System.Guid> where T : class, IEntity<System.Guid>;

public abstract class EfCoreRepository<T> : EfCoreIdentifierRepository<T, System.Guid>, IRepository<T> where T : class, IEntity<System.Guid>;