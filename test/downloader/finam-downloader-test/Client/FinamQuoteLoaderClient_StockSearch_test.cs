using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using database.entity;
using downloader_interactor;
using finam_downloader;
using migration;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http;
using finam_downloader.models;

namespace finam_downloader_test
{
    public class FinamQuoteLoaderClient_StockSearch_test
    {
        private readonly ITestOutputHelper _output;

        public FinamQuoteLoaderClient_StockSearch_test(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task StockSearchTest()
        {
            #region Array

            var client = new DefaultHttpClientFactory().CreateClient();
            client.BaseAddress = new Uri("http://127.0.0.1:7001/");

            IFinamQuoteLoaderClient _downloader =
                new FinamQuoteLoaderClient(_output.BuildLoggerFor<FinamQuoteLoaderClient>(), client);

            var expected_list = new List<FinamStockDTO>()
            {
                new FinamStockDTO()
                {
                    Id = 20569,
                    Name = "Apple Inc.",
                    Code = "AAPL",
                    Market = "USA"
                },
                new FinamStockDTO()
                {
                    Id = 874159,
                    Name = "Advance Auto Parts Inc Advance Auto Parts Inc W/I",
                    Code = "AAP",
                    Market = "USA"
                }
            };

            #endregion

            #region Act

            var asserted_list = await _downloader.StockSearch("USA", "AAP");

            #endregion

            #region Assert

            Assert.Equal(asserted_list.Count(), expected_list.Count);

            foreach (FinamStockDTO expectedStock in expected_list)
            {
                var assertedStock = asserted_list.Single(ast =>
                    ast.Id == expectedStock.Id && ast.Market == expectedStock.Market );
                AssertStock(assertedStock, expectedStock);
            }

            #endregion
        }

        private void AssertStock(FinamStockDTO asserted, FinamStockDTO expected)
        {
            Assert.Equal(asserted.Id, expected.Id);
            Assert.Equal(asserted.Code, expected.Code);
            Assert.Equal(asserted.Name, expected.Name);
            Assert.Equal(asserted.Market, expected.Market);
        }
    }
}
