using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Extensions;
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
            request.Contents = request.Contents.ToBase64(); // This is needed to preserve formatting
            return (await _httpService.HttpPost<CreateDocResponse>("wiki", request)).Doc;
        }

        public async Task<Documentation> Edit(UpdateDocRequest request)
        {
            request.Contents = request.Contents.ToBase64(); // This is needed to preserve formatting
            return (await _httpService.HttpPut<CreateDocResponse>("wiki", request)).Doc;
        }

        public async Task<string> Delete(int id)
        {
            return (await _httpService.HttpDelete<DeleteDocResponse>("wiki", id)).Status;
        }

        public async Task<Documentation> GetById(int id)
        {
            var item = await _httpService.HttpGet<CreateDocResponse>($"wiki/{id}");
            
            if (item == null || item.Doc == null)
            {
                _logger.LogError($"Was unable to retrieve item with ID: {id}");
                return new();
            }
            
            item.Doc.Contents = item.Doc.Contents.FromBase64(); // This is needed to make it usable on client side
            return item.Doc;
        }

        public async Task<List<Documentation>> GetAll()
        {
            var items = await _httpService.HttpGet<List<Documentation>>("wiki/list");
            return items;
        }
    }
}