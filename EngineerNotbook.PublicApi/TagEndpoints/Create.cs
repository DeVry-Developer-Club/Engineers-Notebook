using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared.Authorization;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Endpoint for creating a new record of <see cref="Tag"/>
    /// </summary>
    [Authorize(Roles = Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Create : BaseAsyncEndpoint
        .WithRequest<CreateTagRequest>
        .WithResponse<CreateTagResponse>
    {
        private readonly IAsyncRepository<Tag> _repo;

        public Create(IAsyncRepository<Tag> repo)
        {
            _repo = repo;
        }

        [HttpPost("api/tags")]
        [SwaggerOperation(
            Summary = "Creates a new Tag",
            Description = "Creates a new Tag",
            OperationId = "tags.create",
            Tags=new[]{"TagEndpoints"})]
        public override async Task<ActionResult<CreateTagResponse>> HandleAsync(CreateTagRequest request, CancellationToken cancellationToken = default)
        {
            var tag = new Tag
            {
                Name = request.Name,
                TagType = request.TagType
            };

            tag = await _repo.AddAsync(tag, cancellationToken);
            
            TagDto dto = new()
            {
                Id = tag.Id,
                Name = request.Name,
                TagType = request.TagType
            };

            var response = new CreateTagResponse(request.CorrelationId());
            response.Result = dto;

            return response;
        }
    }
}