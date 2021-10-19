using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Endpoint for retrieving a <see cref="Tag"/> by ID
    /// </summary>
    public class GetById : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<GetByIdTagResponse>
    {
        private readonly IAsyncRepository<Tag> _context;

        public GetById(IAsyncRepository<Tag> context)
        {
            _context = context;
        }

        [HttpGet("api/tags/{tagId}")]
        [SwaggerOperation(
            Summary = "Get a tag by Id",
            Description = "Get a tag by Id",
            OperationId = "tags.GetById",
            Tags = new[] { "TagEndpoints" })]
        public override async Task<ActionResult<GetByIdTagResponse>> HandleAsync(int tagId,
            CancellationToken cancellationToken = default)
        {
            var response = new GetByIdTagResponse();

            var item = await _context.GetByIdAsync(tagId, cancellationToken);

            if (item is null)
                return NotFound();

            response.Tag = new Tag
            {
                Id = item.Id,
                Name = item.Name,
                TagType = item.TagType
            };

            return Ok(response);
        }
    }
}