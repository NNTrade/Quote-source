using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace database.entity
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }

        public Market.Enum MarketId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public int FinamId { get; set; }

        public bool Exclude { get; set; }

        #region relation

        public Market Market { get; set; }
        public IList<StockTimeFrame> StockTimeFrames { get; set; }

        #endregion

        public static void OnModelCreating(EntityTypeBuilder<Stock> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasIndex(e => new { e.MarketId, e.Code });
            entityTypeBuilder
                .HasOne<Market>(e=>e.Market)
                .WithMany(e => e.Stocks)
                .HasForeignKey(e => e.MarketId)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.Property(s => s.Exclude).HasDefaultValue(false);
        }

        public static Stock Build(QuoteSourceDbContext dbContext, Market.Enum marketId, string code, string name,
            int finamId)
        {
            return new Stock()
            {
                Code = code,
                Name = name,
                FinamId = finamId,
                MarketId = marketId,
                Market = dbContext.Market.Single(m => m.Id == marketId)
            };
        }
    }
}
