using System.Collections.Generic;
using System.Linq;
using finam_downloader.models;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace finam_downloader_test.models
{
    public class FinamStockDTOTest
    {
        private readonly ITestOutputHelper output;

        public FinamStockDTOTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void FromJson()
        {
            #region Array

            string json = @"[
                {
                    'id': 20637,
                    'name': 'РОСИНТЕРао',
                    'code': 'ROST',
                    'market': 'SHARES'
                },
                {
                    'id': 81820,
                    'name': 'АЛРОСА ао',
                    'code': 'ALRS',
                    'market': 'SHARES'
                }]";

            var expectedDtos = new List<FinamStockDTO>()
            {
                new FinamStockDTO() { Code = "ROST", Market = "SHARES", Id = 20637, Name = "РОСИНТЕРао" },
                new FinamStockDTO() { Code = "ALRS", Market = "SHARES", Id = 81820, Name = "АЛРОСА ао" }
            };

            #endregion

            #region Act

            var assertedDtos = JsonConvert.DeserializeObject<IEnumerable<FinamStockDTO>>(json);

            #endregion

            #region Assert

            Assert.Equal(expectedDtos.Count, assertedDtos.Count());
            foreach (FinamStockDTO _expectedDto in expectedDtos)
            {
                assertFinamStockDTO(assertedDtos.Single(d => d.Id == _expectedDto.Id), _expectedDto);
            }

            #endregion
        }

        private void assertFinamStockDTO(FinamStockDTO expected, FinamStockDTO asserted)
        {
            Assert.Equal(expected.Id, asserted.Id);
            Assert.Equal(expected.Code, asserted.Code);
            Assert.Equal(expected.Market, asserted.Market);
            Assert.Equal(expected.Name, asserted.Name);
        }
    }
}
