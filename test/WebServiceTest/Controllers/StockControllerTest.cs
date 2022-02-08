using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AppCore;
using AppCore.models;
using database;
using downloader_interactor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace WebServiceTest.Controllers
{
    public class StockControllerTest
    {
        protected readonly ITestOutputHelper _output;
        private IAsyncQuoteSource _asyncQuoteSource { get; set; }
        private HttpClient _client;

        public StockControllerTest(ITestOutputHelper output)
        {
            _output = output;
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                    });
                    builder.ConfigureServices(sc =>
                    {
                        sc.Remove(sc.Single(s => s.ServiceType == typeof(QuoteSourceDbContext)));
                        sc.Remove(sc.Single(s => s.ServiceType == typeof(IDownloader)));
                        sc.Remove(sc.Single(s => s.ServiceType == typeof(IAsyncQuoteSource)));
                        sc.Remove(sc.Single(s => s.ServiceType == typeof(IIniter)));
                        sc.AddScoped<IIniter>(sp=>Substitute.For<IIniter>());
                        sc.AddScoped<IAsyncQuoteSource>(provider => _asyncQuoteSource);
                    });
                });
            _client = application.CreateClient();
        }


        [Theory]
        [InlineData("/api/market/3/Stock")]
        [InlineData("/api/market/3/Stock?code=")]
        public async Task GetWith_no_code(string url)
        {
            #region Array

            List<StockDTO> expectedDTOs = new List<StockDTO>()
            {
                new StockDTO()
                {
                    Code = "AAPL",
                    Id = 1,
                    MarketId = 3,
                    Name = "Apple"
                },
                new StockDTO()
                {
                    Id = 2,
                    Code = "BAPL",
                    MarketId = 3,
                    Name = "Bpple"
                },
            };
            _asyncQuoteSource = Substitute.For<IAsyncQuoteSource>();
            _asyncQuoteSource.StockSearch(3, "").Returns(info => Task.FromResult<IList<StockDTO>>(expectedDTOs));
            #endregion

            #region Act

            var response = await _client.GetAsync(url);

            #endregion

            #region Assert
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _asyncQuoteSource.Received().StockSearch(3, "");

            #endregion
        }

        [Theory]
        [InlineData("/api/market/3/Stock?code=123")]
        public async Task GetWith_code(string url)
        {
            #region Array

            List<StockDTO> expectedDTOs = new List<StockDTO>()
            {
                new StockDTO()
                {
                    Code = "AAPL",
                    Id = 1,
                    MarketId = 3,
                    Name = "Apple"
                },
                new StockDTO()
                {
                    Id = 2,
                    Code = "BAPL",
                    MarketId = 3,
                    Name = "Bpple"
                },
            };
            _asyncQuoteSource = Substitute.For<IAsyncQuoteSource>();
            _asyncQuoteSource.StockSearch(3, "123").Returns(info => Task.FromResult<IList<StockDTO>>(expectedDTOs));
            #endregion

            #region Act

            var response = await _client.GetAsync(url);

            #endregion

            #region Assert

            var response_payload = response.Content.ReadAsStringAsync().Result;
            _output.WriteLine(response_payload);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _asyncQuoteSource.Received().StockSearch(3, "123");
            Assert.Equal("[{\"id\":1,\"marketId\":3,\"code\":\"AAPL\",\"name\":\"Apple\"},{\"id\":2,\"marketId\":3,\"code\":\"BAPL\",\"name\":\"Bpple\"}]", response_payload);
            #endregion
        }
    }
}
