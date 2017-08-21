using Newtonsoft.Json;

namespace toofz
{
    public sealed class CategoryItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}