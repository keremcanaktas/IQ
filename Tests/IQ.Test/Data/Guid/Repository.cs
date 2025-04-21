namespace IQ.Test.Data.Guid;

public interface IRepository<T> : IReadonlyRepository<T, System.Guid> where T : class, IEntity<System.Guid>;

public abstract class Repository<T> : IdentifierRepository<T, System.Guid>, IRepository<T> where T : class, IEntity<System.Guid>;