using EngineerNotebook.Core.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.Infrastructure.Utility
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {
        private readonly HttpClient _client;
        private readonly string _converterApi;
        private ILogger<HtmlToPdfConverter> _logger;
        public HtmlToPdfConverter(IConfiguration config, ILogger<HtmlToPdfConverter> logger)
        {
            _logger = logger;
            _converterApi = config.GetValue<string>("ConverterAPI");
            _client = new();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        }
        
        public async Task<byte[]> HtmlToPdf(string html)
        {
            var response = await _client.PostAsync(_converterApi, new StringContent(html, Encoding.UTF8, "text/html"));

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsByteArrayAsync();
            
            _logger.LogWarning($"Failed to convert HTML: {response.StatusCode} | {response.ReasonPhrase}");
            
            return null;
        }
    }
}