using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Infrastructure.Identity;
using EngineerNotebook.Shared.Authorization;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    [Authorize(Roles = Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Create : BaseAsyncEndpoint
        .WithRequest<CreateDocRequest>
        .WithResponse<CreateDocResponse>
    {
        private readonly IAsyncRepository<Documentation> _docRepo;

        public Create(IAsyncRepository<Documentation> docRepo)
        {
            _docRepo = docRepo;
        }

        [HttpPost("api/wiki")]
        [SwaggerOperation(
            Summary = "Creates a new Documentation Page",
            Description = "Creates a new Documentation Page",
            OperationId = "wiki.create",
            Tags = new[]{"WikiEndpoints"})
        ]
        public override async Task<ActionResult<CreateDocResponse>> HandleAsync(CreateDocRequest request, CancellationToken cancellationToken = default)
        {
            string username = HttpContext.User.Identity?.Name ?? "";

            var doc = new Documentation
            {
                Title = request.Title,
                Description = request.Description,
                Contents = request.Contents,
                CreatedByUserId = username
            };

            doc = await _docRepo.AddAsync(doc, cancellationToken);

            var dto = new DocDto
            {
                Id = doc.Id,
                Title = doc.Title,
                Description = doc.Description,
                Contents = doc.Contents,
                CreatedByUserId = doc.CreatedByUserId,
            };

            var response = new CreateDocResponse(request.CorrelationId());
            response.Doc = dto;

            return response;
        }
    }
}