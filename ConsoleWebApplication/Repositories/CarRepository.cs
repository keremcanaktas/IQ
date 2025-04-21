using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsoleWebApplication.Repositories
{
    public class CarRepository : EfCoreRepository<Car, int>, ICarRepository
    {
        private readonly ILogger<CarRepository> _logger;

        public CarRepository(ApplicationDbContext context, ILogger<CarRepository> logger) 
            : base(context)
        {
            _logger = logger;
        }

        public async Task<IReadOnlyList<Car>> GetByBrandAsync(
            string brand, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbSet
                    .Where(c => c.Brand == brand)
                    .OrderBy(c => c.Model)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cars by brand {Brand}", brand);
                throw;
            }
        }

        public async Task<IReadOnlyList<Car>> GetAvailableCarsAsync(
            CancellationToken cancellationToken = default)
        {
            return await Query()
                .Where(c => c.IsAvailable)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
} 