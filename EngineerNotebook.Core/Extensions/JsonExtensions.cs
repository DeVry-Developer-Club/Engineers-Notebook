using System.Text.Json;

namespace EngineerNotebook.Core.Extensions
{
    public static class JsonExtensions
    {
        public static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Convert from JSON to <typeparamref name="T"/>
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Object representation of <typeparamref name="T"/> from JSON</returns>
        public static T FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json, _jsonOptions);

        /// <summary>
        /// Convert an instance of <typeparamref name="T"/> into JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToJson<T>(this T obj) =>
            JsonSerializer.Serialize<T>(obj, _jsonOptions);
    }
}