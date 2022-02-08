using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using database.entity;
using finam_downloader;
using AppCore;
using finam_downloader.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using migration;
using Xunit;
using Xunit.Abstractions;
using IDownloader = downloader_interactor.IDownloader;

namespace finam_downloader_test
{
    public class FinamQuoteLoaderClient_QuoteDownload_test
    {
        private readonly ITestOutputHelper _output;

        public FinamQuoteLoaderClient_QuoteDownload_test(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task DownloadTest()
        {
            #region Array

            var client = new DefaultHttpClientFactory().CreateClient();
            client.BaseAddress = new Uri("http://127.0.0.1:7001/");

            IFinamQuoteLoaderClient _downloader = new FinamQuoteLoaderClient(_output.BuildLoggerFor<FinamQuoteLoaderClient>(),client);

            var expected_list = DownloadTestData.TestData();

            #endregion

            #region Act

            var asserted_list = await _downloader.Download("USA", 20569,"MINUTES15",
                new DateTime(2020, 2, 3),
                new DateTime(2020, 2, 3));

            #endregion

            #region Assert

            Assert.Equal(asserted_list.Count(), expected_list.Count());

            foreach (FinamQuoteDTO expectedQuote in expected_list)
            {
                var assertedQuote = asserted_list.Single(ast =>
                    ast.DateTime == expectedQuote.DateTime);
                AssertQuote(assertedQuote, expectedQuote);
            }

            #endregion
        }

        private void AssertQuote(FinamQuoteDTO asserted, FinamQuoteDTO expected)
        {
            Assert.Equal(asserted.DateTimeJson, expected.DateTimeJson);
            Assert.Equal(asserted.DateTime, expected.DateTime);
            Assert.Equal(expected.Open, asserted.Open);
            Assert.Equal(expected.High, asserted.High);
            Assert.Equal(expected.Low, asserted.Low);
            Assert.Equal(expected.Close, asserted.Close);
            Assert.Equal(expected.Volume, asserted.Volume);
        }

        [Fact]
        public async Task CheckUTC()
        {
            #region Array

            using var dbContext = new ContextFactory().CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            var client = new DefaultHttpClientFactory().CreateClient();
            client.BaseAddress = new Uri("http://127.0.0.1:7001/");

            IFinamQuoteLoaderClient _downloader = new FinamQuoteLoaderClient(_output.BuildLoggerFor<FinamQuoteLoaderClient>(),client);

            #endregion

            #region Act

            var asserted_list = await _downloader.Download("SHARES", 16842,"MINUTES15",
                new DateTime(2022, 2, 3),
                new DateTime(2022, 2, 3));

            #endregion

            #region Assert

            asserted_list = asserted_list.OrderBy(a => a.DateTime);
            _output.WriteLine($"В Москве торги открываются в 10, следовательноп гринвичу это 7 (10-3)");
            Assert.Equal("2022-02-03T07:00:00", asserted_list.ElementAt(0).DateTimeJson);
            Assert.Equal(new DateTime(2022,2,3,7,0,0), asserted_list.ElementAt(0).DateTime);

            #endregion
        }
    }
}
