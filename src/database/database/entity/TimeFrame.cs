using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace database.entity
{
    public class TimeFrame
    {
        public enum Enum
        {
            //Year = 11,
            Monthly = 10,
            Weekly = 9,
            Daily = 8,
            //H4 = 7,
            Hour = 6,
            Minute30 = 5,
            Minute15 = 4,
            Minute10 = 3,
            Minute5 = 2,
            Minute = 1
        }

        public Enum Id { get; set; }
        public string Name { get; set; }

        public IList<StockTimeFrame> StockTimeFrames { get; set; }
        public static void OnModelCreating(EntityTypeBuilder<TimeFrame> entityTypeBuilder)
        {
            entityTypeBuilder.HasData(
                Enum.GetValues(typeof(Enum))
                    .Cast<Enum>()
                    .Select(e =>
                        new TimeFrame() { Id = e, Name = e.ToString() })
            );
        }
    }
}
