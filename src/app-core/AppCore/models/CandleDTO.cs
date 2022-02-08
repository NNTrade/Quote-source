using System;
using database.entity;

namespace AppCore.models
{
    public class CandleDTO
    {
        public DateTime StartDateTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
    }

    public static class CandleExtension
    {
        public static CandleDTO ToDTO(this Quote quote)
        {
            return new CandleDTO()
            {
                StartDateTime = quote.CandleStart,
                Open = quote.Open,
                Close = quote.Close,
                High = quote.High,
                Low = quote.Low,
                Volume = quote.Volume
            };
        }
    }
}