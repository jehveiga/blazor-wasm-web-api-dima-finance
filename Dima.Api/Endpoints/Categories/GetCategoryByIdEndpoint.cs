using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

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
            [FromRoute] long id,
            [FromServices] ICategoryHandler handler)
        {
            GetCategoryByIdRequest request = new()
            {
                UserId = "teste@veiga.io",
                Id = id
            };

            Response<Category?> result = await handler.GetByIdAsync(request);
            return result.IsSucess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
