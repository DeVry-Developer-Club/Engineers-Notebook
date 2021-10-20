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
    /// <summary>
    /// Endpoint for deleting a <see cref="Documentation"/> by ID
    /// </summary>
    [Authorize(Roles = Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Delete : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<DeleteDocResponse>
    {
        private readonly IAsyncRepository<Documentation> _context;

        public Delete(IAsyncRepository<Documentation> context)
        {
            _context = context;
        }

        [HttpDelete("api/wiki/{docId}")]
        [SwaggerOperation(
            Summary = "Deletes a Documentation Record",
            Description = "Deletes a Documentation Record",
            OperationId = "wiki.Delete",
            Tags = new[]{"WikiEndpoints"})]
        public override async Task<ActionResult<DeleteDocResponse>> HandleAsync(int docId, CancellationToken cancellationToken = default)
        {
            var response = new DeleteDocResponse();

            var itemToDelete = await _context.GetByIdAsync(docId, cancellationToken);

            if (itemToDelete is null)
                return NotFound();

            await _context.DeleteAsync(itemToDelete, cancellationToken);

            return Ok(response);
        }
    }
}