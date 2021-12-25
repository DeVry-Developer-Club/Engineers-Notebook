using System.Text;
using System.Text.Json;
using EngineerNotebook.Shared;

namespace EngineerNotebook.Editor.Services;

public class HttpService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public HttpService(HttpClient client, BaseUrlConfiguration config)
    {
        _httpClient = client;
        _apiUrl = config.ApiBase;
    }

    public async Task<T?> HttpGet<T>(string url)
        where T : class
    {
        var result = await _httpClient.GetAsync($"{_apiUrl}{url}");
        if (!result.IsSuccessStatusCode)
            return null;

        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T?> HttpDelete<T>(string uri, string id)
        where T : class
    {
        var result = await _httpClient.DeleteAsync($"{_apiUrl}{uri}/{id}");
        if (!result.IsSuccessStatusCode)
            return null;
        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T?> HttpPost<T>(string uri, object dataToSend)
        where T : class
    {
        var content = ToJson(dataToSend);
        var result = await _httpClient.PostAsync($"{_apiUrl}{uri}", content);
        if (!result.IsSuccessStatusCode)
            return null;

        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T?> HttpPut<T>(string uri, object dataToSend)
        where T : class
    {
        var content = ToJson(dataToSend);

        var result = await _httpClient.PutAsync($"{_apiUrl}{uri}", content);
        if (!result.IsSuccessStatusCode)
            return null;

        return await FromHttpResponseMessage<T>(result);
    }

    private StringContent ToJson(object obj) => new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    private async Task<T?> FromHttpResponseMessage<T>(HttpResponseMessage result)
        => JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
}