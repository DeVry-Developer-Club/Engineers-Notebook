using System.Threading;
using EngineerNotebook.Shared.Endpoints.Guide;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MongoDB.Driver;
using Tag = EngineerNotebook.Shared.Models.Tag;

namespace EngineerNotebook.PublicApi.Endpoints;

public static class GuideEndpoints
{
    public static IEndpointRouteBuilder AddGuideEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/guide",
            async ([FromBody]GetByTagsRequest request, 
                [FromServices] IAsyncRepository<Documentation> docRepo, 
                [FromServices] IAsyncRepository<Tag> tagRepo,
                [FromServices] IHtmlToPdfConverter htmlToPdfConverter, 
                [FromServices] IRazorToString razor, CancellationToken cancellationToken) =>
            {
                int required = Math.Max((int) Math.Ceiling(request.TagIds.Count * 0.68f), 2);
                
                // Retrieve all records which have the specified tags (at least contains)
                var filter = Builders<Documentation>.Filter.ElemMatch(x => x.Tags, x => request.TagIds.Contains(x));

                var matchingDocs = await docRepo.Find(filter, cancellationToken);

                var tagIds = matchingDocs.Select(x => x.Tags).SelectMany(x => x).Distinct().ToList();
                
                // TODO: Figure out the way of doing this in mongodb query language
                var tags = await tagRepo.Get(cancellationToken);
                tags = tags.Where(x => tagIds.Contains(x.Id)).ToList();

                List<string> pages = new();
                foreach (var doc in matchingDocs)
                {
                    // The required number is an attempt to find the "most" fit guide
                    if (doc.Tags.Intersect(request.TagIds).Count() < required)
                        continue;
                    
                    var viewModel = new DocumentationViewModel
                    {
                        Doc = doc,
                        Tags = tags.Where(x => doc.Tags.Contains(x.Id)).ToList()
                    };

                    pages.Add(await razor.ToViewString("Doc", viewModel));
                }

                if (!pages.Any())
                    return Results.NotFound();
                
                // Stitch each page together which multiple lines between
                string compiled = string.Join("<br><br><br>", pages);
                var pdf = await htmlToPdfConverter.HtmlToPdf(compiled);

                return Results.File(pdf, "application/pdf");
            });

        return endpoints;
    }
}