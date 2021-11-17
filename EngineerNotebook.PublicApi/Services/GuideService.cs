namespace EngineerNotebook.PublicApi.Services;

using EngineerNotebook.GrpcContracts.Documents;
using EngineerNotebook.GrpcContracts.Guide;
using EngineerNotebook.Shared.Models;
using Grpc.Core;
using MongoDB.Driver;
using System.Threading.Tasks;

public class GuideService : Guides.GuidesBase
{
    private readonly IAsyncRepository<Documentation> _context;
    private readonly IAsyncRepository<Shared.Models.Tag> _tagRepo;
    private readonly IHtmlToPdfConverter _pdfConverter;
    private readonly IRazorToString _razor;

    public GuideService(IRazorToString razor,
        IHtmlToPdfConverter pdfConverter,
        IAsyncRepository<Documentation> context,
        IAsyncRepository<Shared.Models.Tag> tagRepo)
    {
        _context = context;
        _pdfConverter = pdfConverter;
        _razor = razor;
        _tagRepo = tagRepo;
    }



    public override async Task<GuideResponse> Guide(GetByTagsRequest request, ServerCallContext context)
    {
        int required = (int)Math.Ceiling(request.TagIds.Count * 0.65f);

        // Retrieve all records which have the specified tags
        var filter = Builders<Documentation>.Filter.ElemMatch(x => x.Tags, x => request.TagIds.Contains(x));
        //var results = await _context.Find(x=>x.Tags.Count(y=>x.Tags.Contains(y)) > required, context.CancellationToken);
        var matchingDocs = await _context.Find(filter, context.CancellationToken);

        var tagIds = matchingDocs.Select(x => x.Tags).SelectMany(x => x).Distinct().ToList();

        // TODO: Figure out the way of doing this in mongodb query language.... 
        var tags = await _tagRepo.Get(context.CancellationToken);
        tags = tags.Where(x => tagIds.Contains(x.Id)).ToList();

        List<string> pages = new();
        // Render each individual documentation page
        foreach (var doc in matchingDocs)
        {            
            var viewModel = new DocumentationViewModel
            {
                Doc = doc,
                Tags = tags.Where(x => doc.Tags.Contains(x.Id)).ToList()
            };

            pages.Add(await _razor.ToViewString("Doc", viewModel));
        }

        // stitch each page together with multiple lines inbetween
        string compiled = string.Join("<br><br><br>", pages);

        var pdf = await _pdfConverter.HtmlToPdf(compiled);
        
        return new GuideResponse
        {
            Guide = Google.Protobuf.ByteString.CopyFrom(pdf.ToArray()),
            ContentType = "application/pdf"
        };
    }
}
