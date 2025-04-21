using IQ.Mofy.Core.Abstractions.Fundamentals.Providers;
using Microsoft.EntityFrameworkCore;

namespace ConsoleWebApplication.Data;

public interface IDbContextProvider : IAsyncProvider<DbContext>;