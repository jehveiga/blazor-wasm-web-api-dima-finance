using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapGet(pattern: "/{id}", handler: HandleAsync)
               .WithName("Categories: Get by Id")
               .WithSummary("Busca uma categoria pelo id")
               .WithDescription("Busca uma categoria pelo id")
               .WithOrder(4)
               .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            [FromRoute] long id,
            [FromServices] ICategoryHandler handler)
        {
            GetCategoryByIdRequest request = new()
            {
                UserId = user.Identity?.Name ?? string.Empty,
                Id = id
            };

            Response<Category?> result = await handler.GetByIdAsync(request);
            return result.IsSuccess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}