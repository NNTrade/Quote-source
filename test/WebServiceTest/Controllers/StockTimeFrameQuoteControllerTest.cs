using System;
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
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebService;
using Xunit;
using Xunit.Abstractions;

namespace WebServiceTest.Controllers
{
    public class StockTimeFrameQuoteControllerTest
    {
        protected readonly ITestOutputHelper _output;
        private IAsyncQuoteSource AsyncQuoteSource { get; set; }
        private HttpClient _client;

        public StockTimeFrameQuoteControllerTest(ITestOutputHelper output)
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
                        sc.AddScoped<IAsyncQuoteSource>(provider => AsyncQuoteSource!);
                    });
                });
            _client = application.CreateClient();
        }


        [Theory]
        [InlineData("/api/stock/12/timeframe/4/quote?FromDate=2020-02-02&TillDate=2020-02-03")]
        public async Task Get_Quote(string url)
        {
            #region Array

            var rnd = new Random();
            List<CandleDTO> expectedDTOs = new List<CandleDTO>()
            {
                new CandleDTO()
                {
                    StartDateTime = new DateTime(2020,2,2,1,2,3),
                    Open = rnd.Next()/1000m,
                    High = rnd.Next()/1000m,
                    Low =  rnd.Next()/1000m,
                    Close = rnd.Next()/1000m,
                    Volume =  rnd.Next()/1000m,
                },
                new CandleDTO()
                {
                    StartDateTime = new DateTime(2020,2,3,1,2,4),
                    Open = rnd.Next()/1000m,
                    High = rnd.Next()/1000m,
                    Low =  rnd.Next()/1000m,
                    Close = rnd.Next()/1000m,
                    Volume =  rnd.Next()/1000m,
                }
            };
            AsyncQuoteSource = Substitute.For<IAsyncQuoteSource>();
            AsyncQuoteSource
                .Download(12,4,new DateOnly(2020,2,2),new DateOnly(2020,2,3))
                .Returns(info => Task.FromResult<IList<CandleDTO>>(expectedDTOs));
            #endregion

            #region Act

            var response = await _client.GetAsync(url);

            #endregion

            #region Assert
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            await AsyncQuoteSource.Received().Download(12,4,new DateOnly(2020,2,2),new DateOnly(2020,2,3));

            #endregion
        }

        [Theory]
        [InlineData("/api/stock/12/timeframe/4/quote?FromDate=2020-02-02&TillDate=2020-02-03")]
        public async Task Get_Quote_check_data(string url)
        {
            #region Array

            var rnd = new Random();
            List<CandleDTO> expectedDTOs = new List<CandleDTO>()
            {
                new CandleDTO()
                {
                    StartDateTime = new DateTime(2020,2,2,1,2,3),
                    Open = 123.123m,
                    High = 123.124m,
                    Low =  123.125m,
                    Close = 123.126m,
                    Volume =  123.127m,
                },
                new CandleDTO()
                {
                    StartDateTime = new DateTime(2020,2,3,1,2,4),
                    Open = 123.128m,
                    High = 123.129m,
                    Low =  123.130m,
                    Close = 123.131m,
                    Volume =  123.132m,
                }
            };
            AsyncQuoteSource = Substitute.For<IAsyncQuoteSource>();
            AsyncQuoteSource
                .Download(12,4,new DateOnly(2020,2,2),new DateOnly(2020,2,3))
                .Returns(info => Task.FromResult<IList<CandleDTO>>(expectedDTOs));
            #endregion

            #region Act

            var response = await _client.GetAsync(url);

            #endregion

            #region Assert

            var response_payload = response.Content.ReadAsStringAsync().Result;
            _output.WriteLine(response_payload);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            await AsyncQuoteSource.Received().Download(12,4,new DateOnly(2020,2,2),new DateOnly(2020,2,3));
            Assert.Equal("[{\"startDateTime\":\"2020-02-02T01:02:03\",\"open\":123.123,\"high\":123.124,\"low\":123.125,\"close\":123.126,\"volume\":123.127},{\"startDateTime\":\"2020-02-03T01:02:04\",\"open\":123.128,\"high\":123.129,\"low\":123.130,\"close\":123.131,\"volume\":123.132}]", response_payload);
            #endregion
        }
    }
}
