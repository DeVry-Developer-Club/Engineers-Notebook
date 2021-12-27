using System.Threading;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Endpoints.Doc;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EngineerNotebook.PublicApi.Endpoints;

public static class DocEndpoints
{
    public static IEndpointRouteBuilder AddDocEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/docs",
            async (IAsyncRepository<Documentation> docRepo, CancellationToken cancellationToken) =>
            {
                var items = await docRepo.Get(cancellationToken);

                return items.Select(x => new DocDto
                {
                    Id = x.Id,
                    Contents = x.Contents,
                    Description = x.Description,
                    Tags = x.Tags,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    EditedAt = x.EditedAt,
                    CreatedByUserId = x.CreatedByUserId,
                    EditedByUserId = x.EditedByUserId
                }).ToList();
            });

        endpoints.MapGet("api/docs/{id}",
            async ([FromRoute] string id, 
                IAsyncRepository<Documentation> docRepo,
                CancellationToken cancellationToken) =>
            {
                var result = await docRepo.Find(id, cancellationToken);

                if (result is null || result.Value is null)
                    return Results.NotFound();

                var dto = new DocDto
                {
                    Id = result.Value.Id,
                    Contents = result.Value.Contents,
                    Description = result.Value.Description,
                    Title = result.Value.Title,
                    CreatedAt = result.Value.CreatedAt,
                    EditedAt = result.Value.EditedAt,
                    Tags = result.Value.Tags,
                    CreatedByUserId = result.Value.CreatedByUserId,
                    EditedByUserId = result.Value.EditedByUserId
                };

                return Results.Ok(dto);
            });

        endpoints.MapPost("api/docs",
            async ([FromBody] UpdateDocRequest request, 
                [FromServices] IAsyncRepository<Documentation> docRepo, 
                [FromServices] IAsyncRepository<Tag> tagRepo, 
                HttpContext context, 
                CancellationToken cancellationToken) =>
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
        
        endpoints.MapPost("api/docs/create", async([FromBody] CreateDocRequest request, 
            [FromServices]IAsyncRepository<Documentation> docRepo, 
            [FromServices]IAsyncRepository<Tag> tagRepo, 
            HttpContext context, 
            CancellationToken cancellationToken) =>
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
                return Results.Ok(new DocDto
                {
                    Id = response.Value.Id,
                    Contents = response.Value.Contents,
                    Description = response.Value.Description,
                    Tags = response.Value.Tags,
                    Title = response.Value.Title,
                    CreatedAt = response.Value.CreatedAt,
                    EditedAt = response.Value.EditedAt,
                    CreatedByUserId = response.Value.CreatedByUserId,
                    EditedByUserId = response.Value.EditedByUserId
                });

            return Results.BadRequest(response.ErrorMessage);
        });

        endpoints.MapDelete("api/docs/{id}", async ([FromRoute] string id,
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