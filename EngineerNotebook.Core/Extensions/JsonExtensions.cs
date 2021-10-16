using System.Text.Json;

namespace EngineerNotebook.Core.Extensions
{
    public static class JsonExtensions
    {
        public static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static T FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json, _jsonOptions);

        public static string ToJson<T>(this T obj) =>
            JsonSerializer.Serialize<T>(obj, _jsonOptions);
    }
}