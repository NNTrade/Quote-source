using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace database.entity
{
    public class Quote
    {
        public int Id { get; set; }

        public DateTime CandleStart { get; set; }

        [Column(TypeName = "decimal(15, 9)")]
        public decimal Open { get; set; }

        [Column(TypeName = "decimal(15, 9)")]
        public decimal High { get; set; }

        [Column(TypeName = "decimal(15, 9)")]
        public decimal Low { get; set; }

        [Column(TypeName = "decimal(15, 9)")]
        public decimal Close { get; set; }

        [Column(TypeName = "decimal(19, 9)")]
        public decimal Volume { get; set; }

        public int StockTimeFrameId { get; set; }

        #region relations

        public StockTimeFrame StockTimeFrame { get; set; }

        #endregion

        public static void OnModelCreating(EntityTypeBuilder<Quote> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasIndex(e => new { StockTimeFrame_Id = e.StockTimeFrameId, e.CandleStart })
                .IsUnique();
            entityTypeBuilder
                .HasOne<StockTimeFrame>()
                .WithMany(e => e.Quotes)
                .HasForeignKey(e => e.StockTimeFrameId)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.Property(e => e.CandleStart).HasConversion<DateTime>();
        }
    }
}
