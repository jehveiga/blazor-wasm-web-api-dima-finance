using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions
{
    public class DeleteTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapDelete(pattern: "/{id}", handler: HandleAsync)
               .WithName("Transactions: Delete")
               .WithSummary("Exclui uma transação")
               .WithDescription("Exclui uma transação")
               .WithOrder(3)
               .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            [FromRoute] long id,
            [FromServices] ITransactionHandler handler)
        {
            DeleteTransactionRequest request = new()
            {
                UserId = "teste@veiga.io",
                Id = id
            };

            Response<Transaction?> result = await handler.DeleteAsync(request);
            return result.IsSucess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
