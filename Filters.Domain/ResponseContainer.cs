using System.Text.Json.Serialization;

namespace Filters.Domain
{
    public class ResponseContainer
    {
        [JsonPropertyName("response")]
        public IEnumerable<Person>? Response { get; set; }
    }

}
