using System.Net;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Doc;
using EngineerNotebook.Shared.Extensions;
using EngineerNotebook.Shared.Interfaces;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Editor.Services;

/// <summary>
/// Provides basic CRUD operations for our Doc endpoints
/// </summary>
public class DocService : IDocService
{
    private readonly HttpService _httpService;
    private readonly ILogger<DocService> _logger;

    public DocService(HttpService httpService,
        ILogger<DocService> logger)
    {
        _logger = logger;
        _httpService = httpService;
    }

    /// <summary>
    /// Attempt to create a new record of <see cref="Documentation"/> with the given <paramref name="request"/>
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Should return the exact same info with the addition of the PK generated from the database</returns>
    public async Task<DocDto?> Create(CreateDocRequest request)
    {
        request.Contents = request.Contents.ToBase64(); // this is needed to preserve formatting
        return (await _httpService.HttpPost<DocDto>(Endpoints.DOC_CREATE, request));
    }

    /// <summary>
    /// Attempt to edit an existing record of <see cref="Documentation"/>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<(bool success, HttpStatusCode statusCode)> Edit(UpdateDocRequest request)
    {
        request.Contents = request.Contents.ToBase64(); // this is required to preserve formatting
        return await _httpService.HttpPost(Endpoints.DOC_BASE, request);
    }

    /// <summary>
    /// Attempts to delete an existing record of <see cref="Documentation"/>
    /// </summary>
    /// <param name="id">PK of record to delete</param>
    /// <returns>Status code of 'deleted' if successful</returns>
    public async Task<DeleteResponse> Delete(string id)
    {
        var response = await _httpService.HttpDelete<DeleteResponse>(Endpoints.DOC_BASE, id);

        if (response is null || string.IsNullOrEmpty(response.Status))
            _logger.LogError($"Something went wrong trying to delete Doc with id: {id}");

        return response;
    }

    /// <summary>
    /// Attempt to retrieve a <see cref="Documentation"/> record with <paramref name="id"/>
    /// </summary>
    /// <param name="id">PK of record to retrieve</param>
    /// <returns><see cref="Documentation"/> which has <paramref name="id"/> or null if not found</returns>
    public async Task<DocDto> GetById(string id)
    {
        var item = await _httpService.HttpGet<DocDto>($"{Endpoints.DOC_BASE}/{id}");

        if (item is null)
        {
            Console.WriteLine("Result was null");
            return new();
        }

        item.Contents = item.Contents.FromBase64(); // this is needed to make it usable on client side
        return item;
    }

    /// <summary>
    /// Retrieve alll <see cref="Documentation"/> records from database
    /// </summary>
    /// <returns><see cref="Documentation"/> in the form of a list</returns>
    public async Task<List<DocDto>> GetAll()
        =>  await _httpService.HttpGet<List<DocDto>>(Endpoints.DOC_BASE);
    
}