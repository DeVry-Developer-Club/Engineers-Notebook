using System.Threading;
using EngineerNotebook.Shared.Models;
using MongoDB.Driver;
using Tag = EngineerNotebook.Shared.Models.Tag;

namespace EngineerNotebook.PublicApi;

public class GuideService : IGuideService
{
    private readonly IAsyncRepository<Documentation> _docRepo;
    private readonly IHtmlToPdfConverter _pdfConverter;
    private readonly IRazorToString _razor;
    private readonly IAsyncRepository<Tag> _tagRepo;

    public GuideService(
        IAsyncRepository<Documentation> docRepo,
        IRazorToString razor, 
        IHtmlToPdfConverter pdfConverter, 
        IAsyncRepository<Tag> tagRepo)
    {
        _docRepo = docRepo;
        _razor = razor;
        _pdfConverter = pdfConverter;
        _tagRepo = tagRepo;
    }

    public async Task<byte[]> GetGuide(List<string> found, CancellationToken token = default)
    {
        var tags = await _tagRepo.Get(token);
        return await GetGuide(tags.Where(x => found.Contains(x.Id)).ToList(), token);
    }
    
    public async Task<byte[]> GetGuide(List<Tag> found, CancellationToken token = default)
    {
        var ids = found.Select(x => x.Id).ToList();
        int required = Math.Max((int) Math.Ceiling(found.Count * 0.68f), 2);
        
        // Retrieve all records which have the specified tags (at least contains)
        var filter = Builders<Documentation>.Filter.ElemMatch(x => x.Tags, x => ids.Contains(x));

        var matchingDocs = await _docRepo.Find(filter, token);
        
        // TODO: Figure out the way of doing this in mongodb query language
        List<string> pages = new();
        foreach (var doc in matchingDocs)
        {
            // The required number is an attempt to find the "most" fit guide(s)
            if (doc.Tags.Intersect(ids).Count() < required)
                continue;

            var viewModel = new DocumentationViewModel
            {
                Doc = doc,
                Tags = found.Where(x => doc.Tags.Contains(x.Id)).ToList()
            };
            
            pages.Add(await _razor.ToViewString("Doc", viewModel));
        }

        if (!pages.Any())
            return null;

        string compiled = string.Join("<br><br><br>", pages);
        return await _pdfConverter.HtmlToPdf(compiled);
    }
}