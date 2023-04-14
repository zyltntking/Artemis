using System.Text.Json.Serialization;
using Artemis.Data.Core;
using Artemis.Data.Types;

namespace Artemis.Test.WebApiSnippets
{
    public class ResponseEntity
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        [JsonConverter(typeof(EnumerationJsonConverter<HostType>))]
        public HostType HostType { get; set; } = HostType.Database;
    }
}