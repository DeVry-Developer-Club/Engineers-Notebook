using System.Threading;
using EngineerNotebook.Shared.Endpoints.Tag;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EngineerNotebook.PublicApi.Endpoints;

public static class TagEndpoints
{
    public static IEndpointRouteBuilder AddTagEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/tags/create", [Authorize] async ([FromBody]CreateTagRequest request, 
            [FromServices] IAsyncRepository<Tag> tagRepo,
            CancellationToken cancellationToken) =>
        {
            var tag = new Tag
            {
                Name = request.Name,
                TagType = request.TagType
            };

            var response = await tagRepo.Create(tag, cancellationToken);

            if (response.Success)
                return Results.Ok(response.Value);

            return Results.BadRequest(response.ErrorMessage);
        });

        endpoints.MapPost("api/tags", [Authorize] async ([FromBody]UpdateTagRequest request, 
            [FromServices] IAsyncRepository<Tag> tagRepo,
            CancellationToken cancellationToken) =>
        {
            var find = await tagRepo.Find(request.Id, cancellationToken);

            if (!find.Success)
                return Results.NotFound(find.ErrorMessage);

            Tag item = find.Value;
            item.Name = request.Name;
            item.TagType = request.TagType;

            var response = await tagRepo.Update(item, cancellationToken);

            if (response.Success)
                return Results.Ok();

            return Results.BadRequest(response.ErrorMessage);
        });

        endpoints.MapDelete("api/tags", [Authorize]
            async ([FromBody] string id, [FromServices] IAsyncRepository<Tag> tagRepo, CancellationToken cancellationToken) =>
            {
                var find = await tagRepo.Find(id, cancellationToken);

                if (!find.Success)
                    return Results.NotFound(find.ErrorMessage);

                var response = await tagRepo.Delete(id, cancellationToken);

                if (response.Success)
                    return Results.Ok();

                return Results.BadRequest(response.ErrorMessage);
            });


        endpoints.MapGet("api/tags", async ([FromServices] IAsyncRepository<Tag> tagRepo, CancellationToken cancellationToken) => await tagRepo.Get(cancellationToken));
        endpoints.MapGet("api/tags/{id}",
            async ([FromQuery] string id, [FromServices] IAsyncRepository<Tag> tagRepo, CancellationToken cancellationToken) =>
            {
                var response = await tagRepo.Find(id, cancellationToken);

                if (response.Success)
                    return Results.Ok(response.Value);

                return Results.NotFound(response.ErrorMessage);
            });
        return endpoints;
    }
}