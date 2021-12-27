using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Tag;
using EngineerNotebook.Shared.Interfaces;

namespace EngineerNotebook.Editor.Services;
public class TagService : ITagService
{
    private readonly HttpService _httpService;
    private readonly ILogger<TagService> _logger;

    public TagService(HttpService httpService, ILogger<TagService> logger)
    {
        _httpService = httpService;
        _logger = logger;
    }

    /// <summary>
    /// Attempts to create a new record of <see cref="TagDto"/>
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The same information with the addition of the generated PK</returns>
    public async Task<TagDto?> Create(CreateTagRequest request)
        => (await _httpService.HttpPost<TagResponse>(Endpoints.TAG_CREATE, request))?.Result;

    /// <summary>
    /// Attempts to edit an existing tag record with the values provided in <paramref name="request"/>
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Updated version of <see cref="TagDto"/></returns>
    public async Task<TagDto?> Update(UpdateTagRequest request) =>
        (await _httpService.HttpPut<TagResponse>(Endpoints.TAG_BASE, request))?.Result;

    /// <summary>
    /// Attempts to delete a Tag record from database that has <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The response from deletion</returns>
    public async Task<DeleteResponse?> Delete(string id)
    {
        var response = await _httpService.HttpDelete<DeleteResponse>(Endpoints.TAG_BASE, id);

        if (response is null || string.IsNullOrEmpty(response.Status))
            _logger.LogError($"Something went wrong trying to delete tag with id: {id}");

        return response;
    }

    /// <summary>
    /// Attempt to retrieve a tag record with <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Tag with <paramref name="id"/> or null if not found</returns>
    public async Task<TagDto> GetById(string id)
    {
        var item = await _httpService.HttpGet<TagResponse>($"{Endpoints.TAG_BASE}/{id}");

        if (item is null || item.Result is null)
        {
            _logger.LogError($"Was unable to retrieve tag with id: {id}");
            return new();
        }

        return item.Result;
    }

    /// <summary>
    /// Retrieve all Tag records from database
    /// </summary>
    /// <returns></returns>
    public async Task<List<TagDto>> GetAll()
         => await _httpService.HttpGet<List<TagDto>>(Endpoints.TAG_BASE);
}