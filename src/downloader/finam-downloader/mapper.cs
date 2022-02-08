using System;
using database.entity;

namespace finam_downloader
{
    public static class mapper
    {
        public static string ToFinamTimeFrame(this TimeFrame.Enum timeframe)
        {
            switch (timeframe)
            {
                case TimeFrame.Enum.Monthly:
                    return "MONTHLY";
                case TimeFrame.Enum.Weekly:
                    return "WEEKLY";
                case TimeFrame.Enum.Daily:
                    return "DAILY";
                case TimeFrame.Enum.Hour:
                    return "HOURLY";
                case TimeFrame.Enum.Minute30:
                    return "MINUTES30";
                case TimeFrame.Enum.Minute15:
                    return "MINUTES15";
                case TimeFrame.Enum.Minute10:
                    return "MINUTES10";
                case TimeFrame.Enum.Minute5:
                    return "MINUTES5";
                case TimeFrame.Enum.Minute:
                    return "MINUTES1";
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeframe), timeframe, null);
            }
        }
        public static string ToFinamMarket(this Market.Enum market)
        {
            return market switch
            {
                database.entity.Market.Enum.Forex => "CURRENCIES_WORLD",
                database.entity.Market.Enum.UsaStock => "USA",
                database.entity.Market.Enum.MmvbStock => "SHARES",
                database.entity.Market.Enum.CryptoCurrency => "CRYPTO_CURRENCIES",
                _ => throw new ArgumentOutOfRangeException(nameof(market), market, null)
            };
        }

        public static Market.Enum ParseMarket(this string FinamMarket)
        {
            switch (FinamMarket)
            {
                case "CURRENCIES_WORLD":
                    return database.entity.Market.Enum.Forex;
                case "USA":
                    return database.entity.Market.Enum.UsaStock;
                case "SHARES":
                    return database.entity.Market.Enum.MmvbStock;
                case "CRYPTO_CURRENCIES":
                    return database.entity.Market.Enum.CryptoCurrency;
                default:
                    throw new ArgumentOutOfRangeException(nameof(FinamMarket), FinamMarket, null);
            }
        }
    }
}
