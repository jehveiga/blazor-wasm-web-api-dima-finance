using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions
{
    public class UpdateTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPut(pattern: "/{id}", handler: HandleAsync)
               .WithName("Transactions: Update")
               .WithSummary("Atualiza uma transação")
               .WithDescription("Atualiza uma transação")
               .WithOrder(2)
               .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            [FromRoute] long id,
            [FromBody] UpdateTransactionRequest request,
            [FromServices] ITransactionHandler handler)
        {
            request.UserId = "teste@veiga.io";
            request.Id = id;
            Response<Transaction?> result = await handler.UpdateAsync(request);
            return result.IsSuccess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}