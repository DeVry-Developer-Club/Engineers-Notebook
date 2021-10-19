using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Core.Specifications;
using EngineerNotebook.PublicApi.TagEndpoints;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Endpoint for retrieving all records of <see cref="Documentation"/>
    /// </summary>
    public class List : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<GetByIdDocResponse>
    {
        private readonly IAsyncRepository<Documentation> _context;

        public List(IAsyncRepository<Documentation> context)
        {
            _context = context;
        }

        [HttpGet("api/wiki/list")]
        [SwaggerOperation(
            Summary = "Get all wiki pages",
            Description = "Get all wiki pages",
            OperationId = "wiki.List",
            Tags = new[]{"WikiEndpoints"})]
        public override async Task<ActionResult<GetByIdDocResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var items = await _context.ListAsync(new DocumentationListAllSpecification(), cancellationToken);
            
            if(items is null)
                return NotFound();

            var resultSet = items.Select(x => new DocDto
            {
                Contents = x.Contents,
                Title = x.Title,
                Description = x.Description,
                CreatedByUserId = x.CreatedByUserId,
                EditedByUserId = x.EditedByUserId,
                CreatedAt = x.CreatedAt,
                EditedAt = x.EditedAt,
                Id = x.Id,
                Tags = x.Tags.Select(y => new TagDto
                {
                    Id = y.Id,
                    Name = y.Name,
                    TagType = y.TagType
                }).ToList() ?? null
            });
            
            return Ok(resultSet);
        }
    }
}