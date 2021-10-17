using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class GetById : BaseAsyncEndpoint
        .WithRequest<GetByIdDocRequest>
        .WithResponse<GetByIdDocResponse>
    {
        private readonly IAsyncRepository<Documentation> _context;

        public GetById(IAsyncRepository<Documentation> context)
        {
            _context = context;
        }

        [HttpGet("api/wiki/{DocId}")]
        [SwaggerOperation(
            Summary = "Get a wiki page by Id",
            Description = "Get a wiki page by Id",
            OperationId = "wiki.GetById",
            Tags = new[]{"WikiEndpoints"})]
        public override async Task<ActionResult<GetByIdDocResponse>> HandleAsync(GetByIdDocRequest request, CancellationToken cancellationToken = default)
        {
            var response = new GetByIdDocResponse(request.CorrelationId());

            var item = await _context.GetByIdAsync(request.DocId, cancellationToken);
            
            if (item is null)
                return NotFound();
            
            response.Doc = new()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Contents = item.Contents,
                CreatedByUserId = item.CreatedByUserId,
                EditedByUserId = item.EditedByUserId,
                CreatedAt = item.CreatedAt,
                EditedAt = item.EditedAt,
                Tags = item.Tags
            };

            return Ok(response);
        }
    }
}