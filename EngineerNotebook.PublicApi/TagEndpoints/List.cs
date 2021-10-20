using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Endpoint for retrieving all records of <see cref="Tag"/>
    /// </summary>
    public class List : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<ListTagsResponse>
    {
        private readonly IAsyncRepository<Tag> _repository;
        private readonly IMapper _mapper;

        public List(IAsyncRepository<Tag> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("api/tags")]
        [SwaggerOperation(
            Summary = "List Tags",
            Description = "List Tags",
            OperationId = "tags.List",
            Tags = new[]{"TagEndpoints"})]
        public override async Task<ActionResult<ListTagsResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var items = await _repository.ListAllAsync(cancellationToken);

            if (items is null)
                return NotFound();
            return Ok(items);
        }
    }
}