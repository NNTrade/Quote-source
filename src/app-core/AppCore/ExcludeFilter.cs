using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppCore.models;
using database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppCore
{
    public class ExcludeFilter : IExcludeFilter
    {
        private readonly QuoteSourceDbContext _dbContext;
        private readonly ILogger<ExcludeFilter> _logger;

        public ExcludeFilter(QuoteSourceDbContext dbContext, ILogger<ExcludeFilter> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Add(int stockId)
        {
            var stock = _dbContext.Stock.Single(s => s.Id == stockId);
            stock.Exclude = true;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Add stock (id: {@stockId}) to exclude", stock.Id);
        }

        public async Task<List<StockDTO>> GetExcludes()
        {
            return await _dbContext.Stock.Where(s => s.Exclude).Select(s=>s.ToDto()).ToListAsync();
        }

        public async Task Remove(int stockId)
        {
            var stock = _dbContext.Stock.Single(s => s.Id == stockId);

            stock.Exclude = false;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Remove stock (id: {@stockId}) from exclude", stock.Id);
        }


    }
}
