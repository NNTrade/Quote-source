using Newtonsoft.Json;

namespace finam_downloader.models
{
    public class FinamStockDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }
    }


}
