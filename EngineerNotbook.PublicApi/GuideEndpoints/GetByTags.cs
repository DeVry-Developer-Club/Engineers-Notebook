using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Core.Specifications;
using EngineerNotebook.PublicApi.Guide;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EngineerNotebook.PublicApi.GuideEndpoints
{
    public class GetByTags : BaseAsyncEndpoint
        .WithRequest<GetByTagsRequest>
        .WithoutResponse
    {
        private readonly IAsyncRepository<Documentation> _context;
        private readonly IHtmlToPdfConverter _pdfConverter;
        private readonly IRazorToString _razor;

        public GetByTags(IRazorToString razor, IHtmlToPdfConverter pdfConverter, IAsyncRepository<Documentation> context)
        {
            _razor = razor;
            _pdfConverter = pdfConverter;
            _context = context;
        }

        [HttpGet("/api/guide"), HttpPost("/api/guide")]
        [SwaggerOperation(
            Summary = "Retrieve Wiki Guides from filters",
            Description = "Retrieve Wiki Guides from filters",
            OperationId = "guide.filter",
            Tags = new[]{"GuideEndpoints"})]
        public override async Task<ActionResult> HandleAsync([FromBody] GetByTagsRequest request, CancellationToken cancellationToken = default)
        {
            DocumentationWithFiltersSpecification specification =
                request.TagNames != null
                    ? new DocumentationWithFiltersSpecification(request.TagNames.ToArray())
                    : new DocumentationWithFiltersSpecification(request.TagIds.ToArray());

            // Retrieve all records which have the specified tags
            var results = await _context.ListAsync(specification, cancellationToken);

            List<string> pages = new();
            
            // Render each individual documentation page
            foreach (var doc in results)
                pages.Add(await _razor.ToViewString("Doc", doc));
            
            // stitch each page together with multiple lines inbetween
            string compiled = string.Join("<br/><br/><br/>", pages);
            
            // Convert into PDF
            var pdf = await _pdfConverter.HtmlToPdf(compiled);
            return File(pdf, "application/pdf");
        }
    }
}