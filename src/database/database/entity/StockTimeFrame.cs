using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace database.entity
{
    public class StockTimeFrame
    {
        public int Id { get; set; }

        public int StockId { get; set; }
        public TimeFrame.Enum TimeFrameId { get; set; }

        public DateTime LoadedFrom { get; set; }
        public DateTime LoadedTill { get; set; }

        #region relations

        public Stock Stock { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public IList<Quote> Quotes { get; set; }

        #endregion

        public static void OnModelCreating(EntityTypeBuilder<StockTimeFrame> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasIndex(e =>
                    new { e.StockId, TimeFrame_Id = e.TimeFrameId })
                .IsUnique();
            entityTypeBuilder
                .HasOne<Stock>()
                .WithMany(e => e.StockTimeFrames)
                .HasForeignKey(e => e.StockId)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder
                .HasOne<TimeFrame>()
                .WithMany(e => e.StockTimeFrames)
                .HasForeignKey(e => e.TimeFrameId)
                .OnDelete(DeleteBehavior.Cascade);

            entityTypeBuilder.Property(e => e.LoadedFrom).HasConversion<DateTime>();
            entityTypeBuilder.Property(e => e.LoadedTill).HasConversion<DateTime>();
        }
    }
}
