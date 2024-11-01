using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories
{
    public class DeleteCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapDelete(pattern: "/{id}", handler: HandleAsync)
               .WithName("Categories: Delete")
               .WithSummary("Exclui uma categoria")
               .WithDescription("Exclui uma categoria")
               .WithOrder(3)
               .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            [FromRoute] long id,
            [FromServices] ICategoryHandler handler)
        {
            DeleteCategoryRequest request = new()
            {
                UserId = "teste@veiga.io",
                Id = id
            };

            Response<Category?> result = await handler.DeleteAsync(request);
            return result.IsSucess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
