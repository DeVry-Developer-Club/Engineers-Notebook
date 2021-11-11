namespace EngineerNotebook.PublicApi.Services;
using EngineerNotebook.GrpcContracts.Guide;
using EngineerNotebook.Shared.Models;
using Grpc.Core;
using System.Threading.Tasks;

public class GuideService : Guides.GuidesBase
{
    private readonly IAsyncRepository<Documentation> _context;
    private readonly IHtmlToPdfConverter _pdfConverter;
    private readonly IRazorToString _razor;

    public GuideService(IRazorToString razor,
        IHtmlToPdfConverter pdfConverter,
        IAsyncRepository<Documentation> context)
    {
        _context = context;
        _pdfConverter = pdfConverter;
        _razor = razor;
    }



    public override async Task<GuideResponse> Guide(GetByTagsRequest request, ServerCallContext context)
    {
        int required = (int)Math.Ceiling(request.TagIds.Count * 0.65f);
        
        // Retrieve all records which have the specified tags
        var results = await _context.Find(x=>x.Tags.Count(y=>x.Tags.Contains(y)) > required, context.CancellationToken);

        List<string> pages = new();
        // Render each individual documentation page
        foreach (var doc in results)
            pages.Add(await _razor.ToViewString("Doc", doc));

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
