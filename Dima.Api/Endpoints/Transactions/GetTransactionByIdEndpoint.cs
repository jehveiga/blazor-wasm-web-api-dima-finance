using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions
{
    public class GetTransactionByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapGet(pattern: "/{id}", handler: HandleAsync)
               .WithName("Transactions: Get by Id")
               .WithSummary("Busca uma transação pelo id")
               .WithDescription("Busca uma transação pelo id")
               .WithOrder(4)
               .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            [FromRoute] long id,
            [FromServices] ITransactionHandler handler)
        {
            GetTransactionByIdRequest request = new()
            {
                UserId = "teste@veiga.io",
                Id = id
            };

            Response<Transaction?> result = await handler.GetByIdAsync(request);
            return result.IsSucess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
