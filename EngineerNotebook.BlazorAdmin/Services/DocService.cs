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
    /// <summary>
    /// Provides Basic CRUD operations for our <see cref="Documentation"/> endpoints
    /// </summary>
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
        
        /// <summary>
        /// Attempt to create a new record of <see cref="Documentation"/> with the given <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Should return the exact same info with the addition of the PK generated from the database</returns>
        public async Task<Documentation> Create(CreateDocRequest request)
        {
            request.Contents = request.Contents.ToBase64(); // This is needed to preserve formatting
            return (await _httpService.HttpPost<CreateDocResponse>("wiki", request)).Doc;
        }

        /// <summary>
        /// Attempt to edit an existing record of <see cref="Documentation"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Updated version of the requested data</returns>
        public async Task<Documentation> Edit(UpdateDocRequest request)
        {
            request.Contents = request.Contents.ToBase64(); // This is needed to preserve formatting
            return (await _httpService.HttpPut<CreateDocResponse>("wiki", request)).Doc;
        }

        /// <summary>
        /// Attempts to delete an existing record of <see cref="Documentation"/>
        /// </summary>
        /// <param name="id">Primary key of a <see cref="Documentation"/> record</param>
        /// <returns>Status code of 'Deleted' if successful</returns>
        public async Task<string> Delete(int id)
        {
            var response = await _httpService.HttpDelete<DeleteDocResponse>("wiki", id);

            if (response == null || string.IsNullOrEmpty(response.Status))
            {
                _logger.LogError($"Something went wrong trying to delete Doc with Id: {id}");
                return null;
            }

            return response.Status;
        }

        /// <summary>
        /// Attempts to retrieve a record of <see cref="Documentation"/> with a given <paramref name="id"/>
        /// </summary>
        /// <param name="id">Primary key of record</param>
        /// <returns>The record, if found. Otherwise null</returns>
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

        /// <summary>
        /// Attempts to retrieve all records of <see cref="Documentation"/> from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Documentation>> GetAll()
        {
            var items = await _httpService.HttpGet<List<Documentation>>("wiki/list");
            return items;
        }
    }
}