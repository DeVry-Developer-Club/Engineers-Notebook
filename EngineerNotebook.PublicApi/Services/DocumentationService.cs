using EngineerNotebook.GrpcContracts.Documents;
using EngineerNotebook.Shared.Models;
using Grpc.Core;

namespace EngineerNotebook.PublicApi.Services;
public class DocumentationService : Documents.DocumentsBase
{
    private readonly IAsyncRepository<Documentation> _context;
    private readonly ILogger<DocumentationService> _logger;
    private readonly IAsyncRepository<Tag> _tagRepo;

    public DocumentationService(IAsyncRepository<Documentation> context, ILogger<DocumentationService> logger,
        IAsyncRepository<Tag> tagRepo)
    {
        _context = context;
        _logger = logger;
        _tagRepo = tagRepo;
    }

    public override async Task GetList(EMPTY request, IServerStreamWriter<DocDto> responseStream, ServerCallContext context)
    {
        var list = await _context.Get(context.CancellationToken);

        foreach(var item in list)
        {
            var dto = new DocDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Contents = item.Contents,
                CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(item.CreatedAt),
                EditedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(item.EditedAt),
                CreatedByUserId = item.CreatedByUserId,
                EditedByUserId = item.EditedByUserId,
            };

            dto.TagIds.AddRange(item.Tags);
            await responseStream.WriteAsync(dto);
        }
    }

    public override async Task<DocDto> Update(UpdateRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;

        if (user is null || !user.Identity.IsAuthenticated)
            return null;

        var doc = await _context.Find(request.Id, context.CancellationToken);

        if (doc is null)
            return null;

        List<Tag> cached = new();

        if(request.TagIds != null)
        {
            var tags = await _tagRepo.Find(x => request.TagIds.Contains(x.Id), context.CancellationToken);
            doc.Value.Tags = tags.Select(x=>x.Id).ToList();
        }

        doc.Value.Title = request.Title;
        doc.Value.EditedByUserId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        doc.Value.Contents = request.Contents;
        doc.Value.Description = request.Description;
        doc.Value.EditedAt = DateTimeOffset.Now;

        await _context.Update(doc.Value, context.CancellationToken);

        var dto = new DocDto
        {
            Id = doc.Value.Id,
            Title = doc.Value.Title,
            Description = doc.Value.Description,
            EditedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(doc.Value.EditedAt),
            CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(doc.Value.CreatedAt),
            CreatedByUserId = doc.Value.CreatedByUserId,
            EditedByUserId = doc.Value.EditedByUserId,
            Contents = doc.Value.Contents
        };

        if (doc.Value.Tags != null)
            dto.TagIds.AddRange(doc.Value.Tags);

        if(cached.Any())
            dto.TagIds.AddRange(cached.Select(x=>x.Id));

        return dto;
    }

    public override async Task<DocDto> GetById(IdRequest request, ServerCallContext context)
    {
        var doc = await _context.Find(request.Id, context.CancellationToken);

        if (doc is null)
            return null;

        var dto = new DocDto
        {
            Id = doc.Value.Id,
            Title = doc.Value.Title,
            Description = doc.Value.Description,
            Contents = doc.Value.Contents,
            CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(doc.Value.CreatedAt),
            EditedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(doc.Value.EditedAt),
            CreatedByUserId = doc.Value.CreatedByUserId,
            EditedByUserId = doc.Value.EditedByUserId,
        };

        dto.TagIds.AddRange(doc.Value.Tags);

        return dto;
    }

    public override async Task<DeleteResponse> Delete(IdRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;

        if(user is null)
            return new DeleteResponse()
            {
                Status = "Failed"
            };

        var doc = await _context.Find(request.Id, context.CancellationToken);

        if (doc is null)
            return new DeleteResponse { Status = "Not Found" };

        await _context.Delete(request.Id, context.CancellationToken);

        return new()
        {
            Status = "Deleted"
        };
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;        

        if (user is null)
        {

            var message = new CreateResponse()
            {               
                Success = false,
                Result = null
            };

            message.Messages.Add("Must be authenticated");
            return message;
        }
        var doc = new Documentation()
        {
            Title = request.Title,
            Description = request.Description,
            Contents = request.Contents,
            CreatedByUserId = user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
            CreatedAt = DateTime.Now        
        };

        await _context.Create(doc, context.CancellationToken);

        DocDto dto = new DocDto
        {
            Id = doc.Id,
            Title = doc.Title,
            Contents = doc.Contents,
            Description = doc.Description,
            CreatedByUserId = doc.CreatedByUserId,
            CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(doc.CreatedAt)
        };

        return new CreateResponse()
        {
            Result = dto,
            Success = true
        };
    }
}
