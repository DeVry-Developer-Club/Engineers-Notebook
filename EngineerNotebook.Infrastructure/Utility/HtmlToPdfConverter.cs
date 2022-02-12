using System.IO;
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

            HttpClientHandler handler = new();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            
            _client = new(handler);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        }
        
        public async Task<byte[]> HtmlToPdf(string html)
        {
            var request = WebRequest.Create(_converterApi) as HttpWebRequest;
            request.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            
            // initialize content
            var contentBuffer = Encoding.UTF8.GetBytes(html);
            request.ContentType = "text/html";
            request.ContentLength = contentBuffer.Length;
            request.Method = "POST";

            await using (var requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(contentBuffer, 0, contentBuffer.Length);
                await requestStream.FlushAsync();
                requestStream.Close();
            }
            
            // Get the response / content
            using (var httpResponse = await request.GetResponseAsync() as HttpWebResponse)
            {
                // check for error status
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogCritical($"Error converting to pdf... {httpResponse.StatusCode} | {httpResponse.StatusDescription}");
                    return null;
                }

                await using (var response = httpResponse.GetResponseStream())
                {
                    await using MemoryStream stream = new();
                    await response.CopyToAsync(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}