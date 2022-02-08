using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using finam_downloader.models;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace finam_downloader_test.models
{
    public class FinamQuoteDTOTest
    {
        private readonly ITestOutputHelper output;

        public FinamQuoteDTOTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        public class ConverterTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2020, 10, 14, 1, 2, 3), "2020-10-14T01:02:03" };
                yield return new object[] { new DateTime(2020, 10, 14, 12, 22, 32), "2020-10-14T12:22:32" };
                yield return new object[] { new DateTime(2020, 10, 14), "2020-10-14T00:00:00" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ConverterTestData))]
        public void CheckDateTime(DateTime expected_dt, string string_dt)
        {
            #region Array

            #endregion

            #region Act

            var asserted_dto = new FinamQuoteDTO();
            asserted_dto.DateTimeJson = string_dt;
            var asserted_dt = asserted_dto.DateTime;

            #endregion

            #region Assert

            Assert.Equal(expected_dt, asserted_dt);

            #endregion
        }

        [Fact]
        public void FromJson()
        {
            #region Array

            string json = @"[
                {
                    'DT': '2021-02-01T00:00:00',
                    'Open': 1.21286,
                    'High': 1.21288,
                    'Low': 1.21286,
                    'Close': 1.212865,
                    'Volume': 7.2312
                },
                {
                    'DT': '2021-02-01T00:15:00',
                    'Open': 1.21289,
                    'High': 1.21289,
                    'Low': 1.21286,
                    'Close': 1.21286,
                    'Volume': 2
                },
                {
                    'DT': '2021-02-01T01:30:00',
                    'Open': 1.21248,
                    'High': 1.2125,
                    'Low': 1.21248,
                    'Close': 1.2125,
                    'Volume': 123
                }]";

            var expectedDT = new List<DateTime>()
            {
                new DateTime(2021, 2, 1),
                new DateTime(2021, 02, 01, 0, 15, 0),
                new DateTime(2021, 02, 01, 1, 30, 0)
            };
            var expectedDtos = new Dictionary<DateTime, FinamQuoteDTO>()
            {
                {
                    new DateTime(2021, 2, 1),
                    new FinamQuoteDTO()
                    {
                        DateTime = new DateTime(2021, 2, 1), Open = 1.21286m, High = 1.21288m, Low = 1.21286m,
                        Close = 1.212865m, Volume = 7.2312m
                    }
                },
                {
                    new DateTime(2021, 02, 01, 0, 15, 0),
                    new FinamQuoteDTO()
                    {
                        DateTime = new DateTime(2021, 02, 01, 0, 15, 0), Open = 1.21289m, High = 1.21289m,
                        Low = 1.21286m, Close = 1.21286m, Volume = 2
                    }
                },
                {
                    new DateTime(2021, 02, 01, 1, 30, 0),
                    new FinamQuoteDTO()
                    {
                        DateTime = new DateTime(2021, 02, 01, 1, 30, 0), Open = 1.21248m, High = 1.2125m,
                        Low = 1.21248m,
                        Close = 1.2125m, Volume = 123
                    }
                }
            };

            #endregion

            #region Act

            var assertedDtos = JsonConvert.DeserializeObject<IEnumerable<FinamQuoteDTO>>(json);

            #endregion

            #region Assert

            Assert.Equal(expectedDtos.Count, assertedDtos.Count());
            foreach (FinamQuoteDTO _assertedDto in assertedDtos)
            {
                assertFinamQuoteDTO(expectedDtos[_assertedDto.DateTime],_assertedDto );
            }

            #endregion
        }

        private void assertFinamQuoteDTO(FinamQuoteDTO expected, FinamQuoteDTO asserted)
        {
            Assert.Equal(expected.DateTime, asserted.DateTime);
            Assert.Equal(expected.Open, asserted.Open);
            Assert.Equal(expected.High, asserted.High);
            Assert.Equal(expected.Low, asserted.Low);
            Assert.Equal(expected.Close, asserted.Close);
            Assert.Equal(expected.Volume, asserted.Volume);
        }
    }
}
