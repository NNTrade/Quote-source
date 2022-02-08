using database.entity;
using Microsoft.EntityFrameworkCore;

namespace database
{
    public class QuoteSourceDbContext:DbContext
    {
        public DbSet<TimeFrame> TimeFrame { get; set; }
        public DbSet<Market> Market { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockTimeFrame> StockTimeFrame { get; set; }
        public DbSet<Quote> Quote { get; set; }
        public QuoteSourceDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            entity.TimeFrame.OnModelCreating(modelBuilder.Entity<TimeFrame>());
            entity.Market.OnModelCreating(modelBuilder.Entity<Market>());
            entity.Stock.OnModelCreating(modelBuilder.Entity<Stock>());
            entity.StockTimeFrame.OnModelCreating(modelBuilder.Entity<StockTimeFrame>());
            entity.Quote.OnModelCreating(modelBuilder.Entity<Quote>());
        }
    }
}
