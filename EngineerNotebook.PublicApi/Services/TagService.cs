namespace EngineerNotebook.PublicApi.Services;
using EngineerNotebook.GrpcContracts.Tags;
using EngineerNotebook.Shared.Models;
using Grpc.Core;
using System.Threading.Tasks;

public class TagService : Tags.TagsBase
{
    private readonly ILogger<TagService> _logger;
    private readonly IAsyncRepository<Tag> _context;

    public TagService(ILogger<TagService> logger, IAsyncRepository<Tag> context)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task<TagDto> Create(CreateRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;

        if (user is null || !user.Identity.IsAuthenticated)
            return null;

        var tag = new Tag
        {
            Name = request.Name,
            TagType = (Shared.Models.TagType)request.TagType
        };

        var result = await _context.Create(tag, context.CancellationToken);
        tag.Id = result.Value.Id;

        TagDto dto = new TagDto
        {
            Id = tag.Id,
            Name = request.Name,
            TagType = request.TagType
        };

        return dto;
    }

    public override async Task<TagDto> Update(UpdateRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;

        if (user is null || !user.Identity.IsAuthenticated)
            return null;

        var tag = await _context.Find(request.Id, context.CancellationToken);

        if (tag.Value is null)
            return null;

        tag.Value.Name = request.Name;
        tag.Value.TagType = (Shared.Models.TagType)request.TagType;

        await _context.Update(tag.Value, context.CancellationToken);

        return new TagDto
        {
            Name = request.Name,
            TagType = request.TagType,
            Id = tag.Value.Id
        };
    }

    public override async Task<DeleteResponse> Delete(IdRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;

        if (user is null || !user.Identity.IsAuthenticated)
            return new DeleteResponse { Status = "Not authenticated" };

        var tag = await _context.Find(request.Id, context.CancellationToken);

        if (tag.Value is null)
            return new DeleteResponse { Status = "Not Found" };

        await _context.Delete(request.Id, context.CancellationToken);

        return new DeleteResponse { Status = "deleted" };
    }

    public override async Task<TagDto> GetById(IdRequest request, ServerCallContext context)
    {
        var tag = await _context.Find(request.Id, context.CancellationToken);
        
        return new TagDto
        {
            Id = tag.Value.Id,
            TagType = (GrpcContracts.Tags.TagType)tag.Value.TagType,
            Name = tag.Value.Name
        };
    }

    public override async Task GetList(EMPTY request, IServerStreamWriter<TagDto> responseStream, ServerCallContext context)
    {
        var items = await _context.Get(context.CancellationToken);

        foreach(var item in items)
        {
            var dto = new TagDto
            {
                Id = item.Id,
                Name = item.Name,
                TagType = (GrpcContracts.Tags.TagType)item.TagType
            };

            await responseStream.WriteAsync(dto);
        }
    }
}
