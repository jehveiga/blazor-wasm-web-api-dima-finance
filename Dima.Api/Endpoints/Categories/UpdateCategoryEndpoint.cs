﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories
{
    public class UpdateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPut(pattern: "/{id}", handler: HandleAsync)
               .WithName("Categories: Update")
               .WithSummary("Atualiza uma categoria")
               .WithDescription("Atualiza uma categoria")
               .WithOrder(2)
               .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            [FromRoute] long id,
            [FromBody] DeteteCategoryRequest request,
            [FromServices] ICategoryHandler handler)
        {
            request.UserId = "teste@veiga.io";
            request.Id = id;
            Response<Category?> result = await handler.UpdateAsync(request);
            return result.IsSucess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
