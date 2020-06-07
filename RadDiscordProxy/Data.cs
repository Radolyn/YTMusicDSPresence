#region

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#endregion

namespace RadDiscordProxy
{
    public partial class Data
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("song")] public string Song { get; set; }

        [JsonProperty("artist")] public string Artist { get; set; }

        [JsonProperty("start")] public ulong? Start { get; set; }

        [JsonProperty("end")] public ulong? End { get; set; }

        [JsonProperty("paused")] public bool Paused { get; set; }
    }

    public partial class Data
    {
        public static Data FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Data>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this Data self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };
    }
}