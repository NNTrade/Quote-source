using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace database.entity
{
    public class Market
    {
        public enum Enum
        {
            Forex = 1,
            UsaStock = 2,
            MmvbStock = 3,
            CryptoCurrency = 4,
        }

        public Enum Id { get; set; }
        public string Name { get; set; }

        public List<Stock> Stocks { get; set; } = new List<Stock>();

        public static void OnModelCreating(EntityTypeBuilder<Market> entityTypeBuilder)
        {
            entityTypeBuilder.HasData(
                System.Enum.GetValues(typeof(Market.Enum))
                    .Cast<Market.Enum>()
                    .Select(e =>
                        new Market() { Id = e, Name = e.ToString() })
            );
        }
    }
}
