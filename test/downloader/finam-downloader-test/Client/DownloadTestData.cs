using System.Collections.Generic;
using finam_downloader.models;
using Newtonsoft.Json;

namespace finam_downloader_test
{
    public static class DownloadTestData
    {
        public static IEnumerable<FinamQuoteDTO> TestData()
        {
          return JsonConvert.DeserializeObject<IEnumerable<FinamQuoteDTO>>(json);
        }

        private static string json = @"[
          {
            'Open': 303.93,
            'High': 309.2,
            'Low': 302.26,
            'Close': 308.8,
            'Volume': 298760,
            'DT': '2020-02-03T17:30:00'
          },
          {
            'Open': 308.85,
            'High': 310.99,
            'Low': 308.81,
            'Close': 310.7,
            'Volume': 238883,
            'DT': '2020-02-03T17:45:00'
          },
          {
            'Open': 310.8,
            'High': 312.61,
            'Low': 310.72,
            'Close': 312.2,
            'Volume': 169726,
            'DT': '2020-02-03T18:00:00'
          },
          {
            'Open': 312.37,
            'High': 312.91,
            'Low': 311.21,
            'Close': 312.39,
            'Volume': 131925,
            'DT': '2020-02-03T18:15:00'
          },
          {
            'Open': 312.46,
            'High': 313.25,
            'Low': 311.95,
            'Close': 312.5,
            'Volume': 154814,
            'DT': '2020-02-03T18:30:00'
          },
          {
            'Open': 312.49,
            'High': 312.53,
            'Low': 311.28,
            'Close': 312.31,
            'Volume': 78883,
            'DT': '2020-02-03T18:45:00'
          },
          {
            'Open': 312.36,
            'High': 312.5,
            'Low': 311.7,
            'Close': 311.77,
            'Volume': 78137,
            'DT': '2020-02-03T19:00:00'
          },
          {
            'Open': 311.78,
            'High': 313.45,
            'Low': 311.75,
            'Close': 312.78,
            'Volume': 123208,
            'DT': '2020-02-03T19:15:00'
          },
          {
            'Open': 312.79,
            'High': 313.48,
            'Low': 312.05,
            'Close': 312.47,
            'Volume': 116445,
            'DT': '2020-02-03T19:30:00'
          },
          {
            'Open': 312.415,
            'High': 312.55,
            'Low': 311.63,
            'Close': 311.93,
            'Volume': 84581,
            'DT': '2020-02-03T19:45:00'
          },
          {
            'Open': 311.93,
            'High': 312.09,
            'Low': 310.46,
            'Close': 310.94,
            'Volume': 118929,
            'DT': '2020-02-03T20:00:00'
          },
          {
            'Open': 310.92,
            'High': 311.28,
            'Low': 310.64,
            'Close': 310.92,
            'Volume': 90505,
            'DT': '2020-02-03T20:15:00'
          },
          {
            'Open': 310.96,
            'High': 311.2,
            'Low': 309.8,
            'Close': 310.15,
            'Volume': 126230,
            'DT': '2020-02-03T20:30:00'
          },
          {
            'Open': 310.14,
            'High': 310.17,
            'Low': 308.66,
            'Close': 308.69,
            'Volume': 137984,
            'DT': '2020-02-03T20:45:00'
          },
          {
            'Open': 308.73,
            'High': 309.01,
            'Low': 306.33,
            'Close': 306.83,
            'Volume': 160085,
            'DT': '2020-02-03T21:00:00'
          },
          {
            'Open': 306.86,
            'High': 307.31,
            'Low': 306.14,
            'Close': 306.87,
            'Volume': 146756,
            'DT': '2020-02-03T21:15:00'
          },
          {
            'Open': 306.8,
            'High': 308.3,
            'Low': 306.64,
            'Close': 307.5,
            'Volume': 93721,
            'DT': '2020-02-03T21:30:00'
          },
          {
            'Open': 307.51,
            'High': 308.01,
            'Low': 307.02,
            'Close': 307.81,
            'Volume': 81629,
            'DT': '2020-02-03T21:45:00'
          },
          {
            'Open': 307.78,
            'High': 308.23,
            'Low': 306.73,
            'Close': 306.8,
            'Volume': 102997,
            'DT': '2020-02-03T22:00:00'
          },
          {
            'Open': 306.78,
            'High': 307.74,
            'Low': 306.72,
            'Close': 307.34,
            'Volume': 98066,
            'DT': '2020-02-03T22:15:00'
          },
          {
            'Open': 307.36,
            'High': 307.96,
            'Low': 307.01,
            'Close': 307.75,
            'Volume': 86009,
            'DT': '2020-02-03T22:30:00'
          },
          {
            'Open': 307.77,
            'High': 307.88,
            'Low': 306.94,
            'Close': 307.09,
            'Volume': 72670,
            'DT': '2020-02-03T22:45:00'
          },
          {
            'Open': 307.09,
            'High': 307.8,
            'Low': 307.04,
            'Close': 307.31,
            'Volume': 85589,
            'DT': '2020-02-03T23:00:00'
          },
          {
            'Open': 307.34,
            'High': 308.9,
            'Low': 307.29,
            'Close': 308.89,
            'Volume': 134820,
            'DT': '2020-02-03T23:15:00'
          },
          {
            'Open': 308.88,
            'High': 309.3,
            'Low': 308.3,
            'Close': 308.3,
            'Volume': 106907,
            'DT': '2020-02-03T23:30:00'
          },
          {
            'Open': 308.28,
            'High': 308.89,
            'Low': 307.36,
            'Close': 308.56,
            'Volume': 211393,
            'DT': '2020-02-03T23:45:00'
          }
        ]";
    }
}
