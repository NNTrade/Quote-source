using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using database.entity;
using finam_downloader.models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace finam_downloader
{
    public class FinamQuoteLoaderClient : IFinamQuoteLoaderClient
    {
        public static string DateTimeFormat = "yyyy-MM-dd";
        private readonly ILogger<FinamQuoteLoaderClient> _logger;
        private readonly HttpClient _httpClient;

        public FinamQuoteLoaderClient(ILogger<FinamQuoteLoaderClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<FinamStockDTO>> StockSearch(string marketName, string code = "")
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "market", marketName },
                { "code", code }
            };
            var endpoint = QueryHelpers.AddQueryString("/stock", queryParams);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            IEnumerable<FinamStockDTO> _finamStocks = null;
            try
            {
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"Get errors while loading data: {response.Content.ReadAsStringAsync().Result}");
                }

                await using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        var json = await streamReader.ReadToEndAsync();
                        _finamStocks = JsonConvert.DeserializeObject<IEnumerable<FinamStockDTO>>(json);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Cannot get response from finam-quote-loader, reason: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get unhandled exception while requesting finam-quote-loader");
                throw;
            }

            if (_finamStocks != null) return _finamStocks;
            _logger.LogError($"Cannot parse data from finam quote service: get null while parsing");
            throw new Exception("Get errors while loading data");
        }

        public async Task<IEnumerable<FinamQuoteDTO>> Download(string marketName, int stockId, string timeFrameName,
            DateTime @from, DateTime till)
        {
            if (@till < @from)
            {
                _logger.LogError($"Try to load when From Date {@from} > Till Date {@till}");
                throw new ArgumentOutOfRangeException($"Cannot load data if From Date {@from} > Till Date {@till}");
            }

            var queryParams = new Dictionary<string, string>()
            {
                { "market", marketName },
                { "idx", stockId.ToString() },
                { "from", @from.ToString(DateTimeFormat) },
                { "till", @till.ToString(DateTimeFormat) },
                { "tf", timeFrameName }
            };
            var endpoint = QueryHelpers.AddQueryString("/quote", queryParams);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            IEnumerable<FinamQuoteDTO> _finamQuotes = null;

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"Get errors while loading data: {response.Content.ReadAsStringAsync().Result}");
                }

                await using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        var json = await streamReader.ReadToEndAsync();
                        _finamQuotes = JsonConvert.DeserializeObject<IEnumerable<FinamQuoteDTO>>(json);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Cannot get response from finam-quote-loader, reason: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get unhandled exception while requesting finam-quote-loader");
                throw;
            }

            if (_finamQuotes != null) return _finamQuotes;
            _logger.LogError($"Cannot parse data from finam quote service: get null while parsing");
            throw new Exception("Get errors while loading data");
        }
    }
}
