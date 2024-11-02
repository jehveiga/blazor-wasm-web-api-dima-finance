using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions
{
    public class GetTransactionsByPeriodEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapGet(pattern: "/", handler: HandleAsync)
               .WithName("Transactions: Get All")
               .WithSummary("Busca uma lista de transações")
               .WithDescription("Busca uma lista de transações")
               .WithOrder(5)
               .Produces<PagedResponse<List<Transaction>>?>();

        private static async Task<IResult> HandleAsync(
            [FromServices] ITransactionHandler handler,
            [FromQuery] DateTime? startDate = default,
            [FromQuery] DateTime? endDate = default,
            [FromQuery] int pageNumber = Configurations.PAGE_NUMBER,
            [FromQuery] int pageSize = Configurations.PAGE_SIZE)
        {
            GetTransactionsByPeriodRequest request = new()
            {
                UserId = "teste@veiga.io",
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartDate = startDate,
                EndDate = endDate
            };

            PagedResponse<List<Transaction>?> result = await handler.GetByPeriodAsync(request);
            return result.IsSucess ? TypedResults.Ok(result)
                                   : TypedResults.BadRequest(result);
        }
    }
}
