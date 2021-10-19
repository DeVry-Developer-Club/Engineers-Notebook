using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Interfaces;
using EngineerNotebook.Shared.Models;
using EngineerNotebook.Shared.Models.Requests;
using EngineerNotebook.Shared.Models.Responses;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.BlazorAdmin.Services
{
    public class TagService : ITagService
    {
        private readonly HttpService _httpService;
        private readonly ILogger<TagService> _logger;
        private readonly string _apiUrl;

        public TagService(HttpService httpService,
            BaseUrlConfiguration config,
            ILogger<TagService> logger)
        {
            _httpService = httpService;
            _logger = logger;
            _apiUrl = config.ApiBase;
        }

        public async Task<Tag> Create(CreateTagRequest request)
        {
            return (await _httpService.HttpPost<CreateTagResponse>("tags", request)).Tag;
        }

        public async Task<Tag> Edit(UpdateTagRequest request)
        {
            return (await _httpService.HttpPut<CreateTagResponse>("tags", request)).Tag;
        }

        public async Task<string> Delete(int id)
        {
            var response = await _httpService.HttpDelete<DeleteTagResponse>("tags", id);

            if (response == null || string.IsNullOrEmpty(response.Status))
            {
                _logger.LogError($"Something went wrong trying to delete Tag with Id: {id}");
                return null;
            }

            return response.Status;
        }

        public async Task<Tag> GetById(int id)
        {
            var item = await _httpService.HttpGet<CreateTagResponse>($"tags/{id}");

            if (item == null || item.Tag == null)
            {
                _logger.LogError($"Was unable to retrieve tag with Id: {id}");
                return new();
            }

            return item.Tag;
        }

        public async Task<List<Tag>> GetAll()
        {
            var items = await _httpService.HttpGet<List<Tag>>("tags");
            return items;
        }
    }
}