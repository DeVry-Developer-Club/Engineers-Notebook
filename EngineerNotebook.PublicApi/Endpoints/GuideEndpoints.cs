using System.Threading;
using EngineerNotebook.Shared.Endpoints.Guide;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EngineerNotebook.PublicApi.Endpoints;

public static class GuideEndpoints
{
    public static IEndpointRouteBuilder AddGuideEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/guide",
            async ([FromBody]GetByTagsRequest request, 
                [FromServices] IGuideService guideService,
                CancellationToken cancellationToken) =>
            {
                var result = await guideService.GetGuide(request.TagIds.ToList(), cancellationToken);
                return Results.File(result, "application/pdf");
            });

        return endpoints;
    }
}