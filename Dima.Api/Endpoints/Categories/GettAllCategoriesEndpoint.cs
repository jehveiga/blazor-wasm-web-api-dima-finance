using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories
{
    public class GettAllCategoriesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapGet(pattern: "/", handler: HandleAsync)
               .WithName("Categories: Get All")
               .WithSummary("Busca uma lista categoria")
               .WithDescription("Busca uma lista categoria")
               .WithOrder(5)
               .Produces<PagedResponse<List<Category>>?>();

        private static async Task<IResult> HandleAsync(
            [FromServices] ICategoryHandler handler,
            [FromQuery] int pageNumber = Configurations.PAGE_NUMBER,
            [FromQuery] int pageSize = Configurations.PAGE_SIZE)
        {
            GetAllCategoriesRequest request = new()
            {
                UserId = "teste@veiga.io",
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            PagedResponse<List<Category>?> result = await handler.GetAllAsync(request);
            return result.IsSucess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
