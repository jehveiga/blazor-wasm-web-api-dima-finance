using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories
{
    public class CreateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost(pattern: "/", handler: HandleAsync)
               .WithName("Categories: Create")
               .WithSummary("Cria uma nova categoria")
               .WithDescription("Cria uma nova categoria")
               .WithOrder(1)
               .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            [FromBody] CreateCategoryRequest request,
            [FromServices] ICategoryHandler handler)
        {
            request.UserId = "teste@veiga.io";

            Response<Category?> result = await handler.CreateAsync(request);
            return result.IsSucess ? TypedResults.Created($"/{result.Data?.Id}", result)
                                   : TypedResults.BadRequest(result);
        }

    }
}
