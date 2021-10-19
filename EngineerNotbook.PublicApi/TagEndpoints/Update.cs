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
    [Authorize(Roles = Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Update : BaseAsyncEndpoint
        .WithRequest<UpdateTagRequest>
        .WithResponse<UpdateTagResponse>
    {
        private readonly IAsyncRepository<Tag> _repo;

        public Update(IAsyncRepository<Tag> repo)
        {
            _repo = repo;
        }

        [HttpPut("api/tags")]
        [SwaggerOperation(
            Summary = "Update an existing Tag Record",
            Description = "Update an existing Tag Record",
            OperationId = "tags.update",
            Tags = new[]{"TagEndpoints"})]
        public override async Task<ActionResult<UpdateTagResponse>> HandleAsync(UpdateTagRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = new UpdateTagResponse(request.CorrelationId());

            var existingItem = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (existingItem == null)
                return NotFound();

            existingItem.Name = request.Name;
            existingItem.TagType = request.TagType;

            await _repo.UpdateAsync(existingItem, cancellationToken);

            var dto = new TagDto
            {
                Name = existingItem.Name,
                Id = existingItem.Id,
                TagType = existingItem.TagType
            };

            response.Tag = dto;
            return response;
        }
    }
}