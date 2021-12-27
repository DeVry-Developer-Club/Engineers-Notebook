using System.Net;
using System.Text;
using EngineerNotebook.Shared;
using Newtonsoft.Json;

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
    
    public async Task<(bool success, HttpStatusCode statusCode)> HttpPost(string uri, object dataToSend)
    {
        var content = ToJson(dataToSend);
        var result = await _httpClient.PostAsync($"{_apiUrl}{uri}", content);

        if (result.IsSuccessStatusCode)
            return (true, result.StatusCode);

        return (false, result.StatusCode);
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

    private StringContent ToJson(object obj) => new(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

    private async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
    {
        string json = await result.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        return JsonConvert.DeserializeObject<T>(json);
    }
}