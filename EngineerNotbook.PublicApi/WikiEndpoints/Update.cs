using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
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
    public class Update : BaseAsyncEndpoint
        .WithRequest<UpdateDocRequest>
        .WithResponse<UpdateDocResponse>
    {
        private readonly IAsyncRepository<Documentation> _context;
        private UserManager<ApplicationUser> _userManager;

        public Update(IAsyncRepository<Documentation> context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPut("api/wiki")]
        [SwaggerOperation(
            Summary="Updates an existing wiki record",
            Description = "Updates an existing wiki record",
            OperationId = "wiki.update",
            Tags = new[]{"WikiEndpoints"})]
        public override async Task<ActionResult<UpdateDocResponse>> HandleAsync(UpdateDocRequest request, CancellationToken cancellationToken = default)
        {
            var response = new UpdateDocResponse(request.CorrelationId());

            var username = HttpContext.User.Identity?.Name ?? "";
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                return Unauthorized();

            var existingItem = await _context.GetByIdAsync(request.Id, cancellationToken);
            
            existingItem.Contents = request.Contents;
            existingItem.Title = request.Title;
            existingItem.Description = request.Description;
            existingItem.EditedAt = DateTimeOffset.UtcNow;
            existingItem.EditedByUserId = userId;

            await _context.UpdateAsync(existingItem, cancellationToken);
            
            var createdUser = await _userManager.FindByIdAsync(existingItem.CreatedByUserId);

            var dto = new DocDto()
            {
                Id = existingItem.Id,
                Title = existingItem.Title,
                Description = existingItem.Description,
                EditedAt = existingItem.EditedAt,
                CreatedAt = existingItem.CreatedAt,
                CreatedByUserId = existingItem.CreatedByUserId,
                EditedByUserId = existingItem.EditedByUserId,
                Contents = existingItem.Contents,
                EditedByUsername = username,
                CreatedByUsername = createdUser.UserName ?? "Unknown"
            };

            response.Doc = dto;
            return response;
        }
    }
}