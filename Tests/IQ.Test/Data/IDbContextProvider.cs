using IQ.Mofy.Core.Abstractions.Fundamentals.Providers;
using Microsoft.EntityFrameworkCore;

namespace IQ.Test.Data;

public interface IDbContextProvider : IAsyncProvider<DbContext>;