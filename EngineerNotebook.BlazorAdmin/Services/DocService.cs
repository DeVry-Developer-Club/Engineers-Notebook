using System.Threading.Tasks;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Interfaces;
using EngineerNotebook.Shared.Models;
using EngineerNotebook.Shared.Models.Requests;
using EngineerNotebook.Shared.Models.Responses;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.BlazorAdmin.Services
{
    public class DocService : IDocService
    {
        private readonly HttpService _httpService;
        private readonly ILogger<DocService> _logger;
        private readonly string _apiUrl;

        public DocService(HttpService httpService,
            BaseUrlConfiguration config,
            ILogger<DocService> logger)
        {
            _httpService = httpService;
            _logger = logger;
            _apiUrl = config.ApiBase;
        }
        
        public async Task<Documentation> Create(CreateDocRequest request)
        {
            return (await _httpService.HttpPost<CreateDocResponse>("wiki", request)).Doc;
        }

        public async Task<Documentation> Edit(UpdateDocRequest request)
        {
            return (await _httpService.HttpPut<CreateDocResponse>("wiki", request)).Doc;
        }

        public async Task<string> Delete(int id)
        {
            return (await _httpService.HttpDelete<DeleteDocResponse>("wiki", id)).Status;
        }

        public async Task<Documentation> GetById(int id)
        {
            var item = await _httpService.HttpGet<CreateDocResponse>($"wiki/{id}");
            return item.Doc;
        }
    }
}