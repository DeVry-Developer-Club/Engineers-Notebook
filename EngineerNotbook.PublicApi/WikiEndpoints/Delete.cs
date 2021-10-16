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

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    [Authorize(Roles = Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Delete : BaseAsyncEndpoint
        .WithRequest<DeleteDocRequest>
        .WithResponse<DeleteDocResponse>
    {
        private readonly IAsyncRepository<Documentation> _context;

        public Delete(IAsyncRepository<Documentation> context)
        {
            _context = context;
        }

        [HttpDelete("api/wiki/{DocId}")]
        [SwaggerOperation(
            Summary = "Deletes a Documentation Record",
            Description = "Deletes a Documentation Record",
            OperationId = "wiki.Delete",
            Tags = new[]{"WikiEndpoints"})]
        public override async Task<ActionResult<DeleteDocResponse>> HandleAsync(DeleteDocRequest request, CancellationToken cancellationToken = default)
        {
            var response = new DeleteDocResponse(request.CorrelationId());

            var itemToDelete = await _context.GetByIdAsync(request.DocId, cancellationToken);

            if (itemToDelete is null)
                return NotFound();

            await _context.DeleteAsync(itemToDelete, cancellationToken);

            return Ok(response);
        }
    }
}