namespace IQ.Test.Data.String;

public interface IRepository<T> : IReadonlyRepository<T, string> where T : class, IEntity<string>;

public abstract class Repository<T> : IdentifierRepository<T, string>, IRepository<T> where T : class, IEntity<string>;