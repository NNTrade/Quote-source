using database.entity;

namespace AppCore.models
{
    public class MarketDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public static class MarketConverters{
        public static Market ToEntity(this MarketDTO market)
        {
            return new Market()
            {
                Id = (Market.Enum)market.Id,
                Name = market.Name
            };
        }

        public static MarketDTO ToDto(this Market market)
        {
            return new MarketDTO()
            {
                Id = (int)market.Id,
                Name = market.Name
            };
        }
    }
}
