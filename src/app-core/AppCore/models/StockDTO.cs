using database.entity;

namespace AppCore.models
{
    public class StockDTO
    {
        public int Id { get; set; }

        public int MarketId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
    }

    public static class StockConverters{
        public static Stock ToEntity(this StockDTO stock)
        {
            return new Stock()
            {
                Id = stock.Id,
                MarketId = (Market.Enum)stock.MarketId,
                Code = stock.Code,
                Name = stock.Name
            };
        }

        public static StockDTO ToDto(this Stock stock)
        {
            return new StockDTO()
            {
                Id = stock.Id,
                MarketId = (int)stock.MarketId,
                Code = stock.Code,
                Name = stock.Name
            };
        }
    }
}
