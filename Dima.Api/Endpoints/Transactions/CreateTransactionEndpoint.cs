using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions
{
    public class CreateTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost(pattern: "/", handler: HandleAsync)
               .WithName("Transactions: Create")
               .WithSummary("Cria uma nova transação")
               .WithDescription("Cria uma nova transação")
               .WithOrder(1)
               .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            [FromBody] CreateTransactionRequest request,
            [FromServices] ITransactionHandler handler)
        {
            request.UserId = "teste@veiga.io";

            Response<Transaction?> result = await handler.CreateAsync(request);
            return result.IsSucess ? TypedResults.Created($"/{result.Data?.Id}", result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
