using System;
using Microsoft.Extensions.Logging;

namespace AppCore
{
    public enum LogEvent
    {
        NoStockTimeFrame = 1,
        NoStock = 2,
        LoadData = 3,
    }
    public static class LogEventExt
    {
        public static EventId GetEventId(this LogEvent s1)
        {
            string name ;
            switch (s1)
            {
                case LogEvent.NoStockTimeFrame:
                    name = "Request new Stock and TimeFrame";
                    break;
                case LogEvent.NoStock:
                    name = "Request unknown stock";
                    break;
                case LogEvent.LoadData:
                    name = "Load data from source";
                    break;
                default:
                    name = $"Enum.{s1.ToString()}";
                    break;
            }
            return new EventId((int)s1, name);
        }

        public static void LogException<T>(this ILogger<T> logger, LogEvent logEvent, Exception ex, string message, params object?[] args)
        {
            logger.LogCritical(logEvent.GetEventId(), ex, message, args);
            throw ex;
        }
    }

}
