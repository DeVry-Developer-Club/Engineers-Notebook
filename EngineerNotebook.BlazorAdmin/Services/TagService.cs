using System.Collections.Generic;
using System.Threading.Tasks;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Tag;
using EngineerNotebook.Shared.Interfaces;
using EngineerNotebook.Shared.Models;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.BlazorAdmin.Services
{
    /// <summary>
    /// Basic service providing CRUD operations for <see cref="Tag"/> endpoints
    /// </summary>
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

        /// <summary>
        /// Attempts to create a new record of <see cref="Tag"/> based on <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Same data with the addition of the primary key generated from the database</returns>
        public async Task<Tag> Create(CreateTagRequest request)
        {
            return (await _httpService.HttpPost<TagResponse>("tags", request)).Result;
        }

        /// <summary>
        /// Attempts to update/edit an existing record of <see cref="Tag"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Updated version of <see cref="Tag"/></returns>
        public async Task<Tag> Edit(UpdateTagRequest request)
        {
            return (await _httpService.HttpPut<TagResponse>("tags", request)).Result;
        }

        /// <summary>
        /// Attempts to delete an existing record of <see cref="Tag"/> with a given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status of "deleted" if successful</returns>
        public async Task<string> Delete(int id)
        {
            var response = await _httpService.HttpDelete<DeleteResponse>("tags", id);

            if (response == null || string.IsNullOrEmpty(response.Status))
            {
                _logger.LogError($"Something went wrong trying to delete Tag with Id: {id}");
                return null;
            }

            return response.Status;
        }

        /// <summary>
        /// Attempts to retrieve an existing record of <see cref="Tag"/> with the given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tag> GetById(int id)
        {
            var item = await _httpService.HttpGet<TagResponse>($"tags/{id}");

            if (item == null || item.Result == null)
            {
                _logger.LogError($"Was unable to retrieve tag with Id: {id}");
                return new();
            }

            return item.Result;
        }

        /// <summary>
        /// Attempts to retrieve all records of <see cref="Tag"/> from the database
        /// </summary>
        /// <returns>List of <see cref="Tag"/></returns>
        public async Task<List<Tag>> GetAll()
        {
            var items = await _httpService.HttpGet<List<Tag>>("tags");
            return items;
        }
    }
}