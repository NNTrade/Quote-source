using System;
using System.Globalization;
using database.entity;
using Newtonsoft.Json;

namespace finam_downloader.models
{
    public class FinamQuoteDTO
    {
        [JsonProperty("DT")]
        public string DateTimeJson { get; set; }

        [JsonIgnore]
        public DateTime DateTime
        {
            get
            {
                return DateTime.ParseExact(DateTimeJson, "yyyy-MM-ddTHH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            set { DateTimeJson = value.ToString("yyyy-MM-ddTHH:mm:ss"); }
        }

        [JsonProperty("Open")]
        public decimal Open { get; set; }

        [JsonProperty("High")]
        public decimal High { get; set; }

        [JsonProperty("Low")]
        public decimal Low { get; set; }

        [JsonProperty("Close")]
        public decimal Close { get; set; }

        [JsonProperty("Volume")]
        public decimal Volume { get; set; }
    }

    public static class FinamQuoteDTOExt
    {
        public static Quote ToEntity(this FinamQuoteDTO finamQuoteDTO)
        {
            return new Quote()
            {
                CandleStart = finamQuoteDTO.DateTime,
                Open = finamQuoteDTO.Open,
                Close = finamQuoteDTO.Close,
                High = finamQuoteDTO.High,
                Low = finamQuoteDTO.Low,
                Volume = finamQuoteDTO.Volume
            };
        }
    }
}
