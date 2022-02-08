using database.entity;

namespace AppCore.models
{
    public class TimeFrameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class TimeFrameConverters{

        public static TimeFrame ToEntity(this MarketDTO timeframe)
        {
            return new TimeFrame()
            {
                Id = (TimeFrame.Enum)timeframe.Id,
                Name = timeframe.Name
            };
        }

        public static TimeFrameDTO ToDto(this TimeFrame timeframe)
        {
            return new TimeFrameDTO()
            {
                Id = (int)timeframe.Id,
                Name = timeframe.Name
            };
        }
    }
}
