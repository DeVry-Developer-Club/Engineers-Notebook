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
    public class Delete : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<DeleteTagResponse>
    {
        private readonly IAsyncRepository<Tag> _repo;

        public Delete(IAsyncRepository<Tag> repo)
        {
            _repo = repo;
        }

        [HttpDelete("api/tags/{tagId}")]
        [SwaggerOperation(
            Summary = "Deletes a Tag Record",
            Description = "Deletes a Tag Record",
            OperationId = "tags.delete",
            Tags = new[] { "TagEndpoints" })]
        public override async Task<ActionResult<DeleteTagResponse>> HandleAsync(int tagId,
            CancellationToken cancellationToken = default)
        {
            var response = new DeleteTagResponse();

            var itemToDelete = await _repo.GetByIdAsync(tagId, cancellationToken);

            if (itemToDelete is null)
                return NotFound();

            await _repo.DeleteAsync(itemToDelete, cancellationToken);

            return Ok(response);
        }
    }
}