using System.Threading;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Doc;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EngineerNotebook.PublicApi.Endpoints;

public static class DocEndpoints
{
    public static IEndpointRouteBuilder AddDocEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/docs", async (IAsyncRepository<Documentation> docRepo, CancellationToken cancellationToken) => 
            await docRepo.Get(cancellationToken));

        endpoints.MapGet("api/docs/{id}",
            async ([FromQuery] string id, IAsyncRepository<Documentation> docRepo,
                CancellationToken cancellationToken) =>
            {
                var result = await docRepo.Find(id, cancellationToken);

                if (result is null)
                    return Results.NotFound();
                
                return Results.Ok(result);
            });

        endpoints.MapPost("api/docs",
            [Authorize] async ([FromBody]UpdateDocRequest request, 
                [FromServices] IAsyncRepository<Documentation> docRepo, 
                [FromServices] IAsyncRepository<Tag> tagRepo, HttpContext context, CancellationToken cancellationToken) =>
            {
                var item = await docRepo.Find(request.Id, cancellationToken);

                if (item is null || !item.Success)
                    return Results.NotFound();

                var entry = item.Value;
                entry.Title = request.Title;
                entry.Description = request.Description;
                entry.Contents = request.Contents;
                entry.EditedAt = DateTimeOffset.Now;

                var editedUserId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
                if (!string.IsNullOrEmpty(editedUserId))
                    entry.EditedByUserId = editedUserId;

                if(request.TagIds is not null && request.TagIds.Any())
                    entry.Tags = request.TagIds;

                var response = await docRepo.Update(entry, cancellationToken);
                
                if(response.Success)
                    return Results.Ok();

                return Results.BadRequest(response.ErrorMessage);
            });
        
        endpoints.MapPost("api/docs/create", [Authorize] async([FromBody] CreateDocRequest request, 
            [FromServices]IAsyncRepository<Documentation> docRepo, 
            [FromServices]IAsyncRepository<Tag> tagRepo, 
            HttpContext context, CancellationToken cancellationToken) =>
        {
            if (request.TagIds is not null && request.TagIds.Any())
            {
                var allTags = await tagRepo.Get(cancellationToken);
                var ids = allTags.Select(x => x.Id).ToArray();
                
                foreach(var tag in request.TagIds)
                    if (!ids.Contains(tag))
                        return Results.BadRequest($"Invalid tag ID {tag}");
            }

            var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

            var doc = new Documentation
            {
                Title = request.Title,
                Description = request.Description,
                Contents = request.Contents,
                CreatedByUserId = userId,
                Tags = request.TagIds
            };

            var response = await docRepo.Create(doc, cancellationToken);

            if (response.Success)
                return Results.Ok(response.Value);

            return Results.BadRequest(response.ErrorMessage);
        });

        endpoints.MapDelete("api/docs/{id}", [Authorize] async ([FromBody] string id,
            IAsyncRepository<Documentation> docRepo,
            CancellationToken cancellationToken) =>
        {
            var item = await docRepo.Find(id, cancellationToken);

            if (!item.Success)
                return Results.NotFound();

            var response = await docRepo.Delete(id, cancellationToken);

            if (response.Success)
                return Results.Ok(new DeleteResponse());

            return Results.BadRequest(response.ErrorMessage);
        });
        
        return endpoints;
    }
}