namespace IQ.Test.Data.Integer;

public interface IRepository<T> : IReadonlyRepository<T, int> where T : class, IEntity<int>;

public abstract class Repository<T> : IdentifierRepository<T, int>, IRepository<T> where T : class, IEntity<int>;