using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class TransactionHandler(AppDbContext dbContext) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                Transaction transaction = new()
                {
                    UserId = request.UserId,
                    CreatedAt = DateTime.Now,
                    CategoryId = request.CategoryId,
                    Amount = request.Amount,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    Title = request.Title,
                    Type = request.Type
                };

                await dbContext.AddAsync(transaction);
                await dbContext.SaveChangesAsync();

                return new Response<Transaction?>(data: transaction, code: 201, message: "Transação criada com sucesso!");
            }
            catch
            {
                return new Response<Transaction?>(data: default, code: 500, message: "Não foi possível criar sua transação");
            }
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            try
            {
                Transaction? transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id &&
                                                                        t.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction?>(data: null, code: 404, message: "Transação não encontrada!");

                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.Amount = request.Amount;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
                transaction.CategoryId = request.CategoryId;

                dbContext.Update(transaction);
                await dbContext.SaveChangesAsync();

                return new Response<Transaction?>(data: transaction, message: "Transação atualizada com sucesso");
            }
            catch
            {
                return new Response<Transaction?>(data: null, code: 500, message: "Nào foi possível recuperar sua transação!");
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                Transaction? transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id &&
                                                                        t.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction?>(data: null, code: 404, message: "Transação não encontrada!");

                dbContext.Remove(transaction);
                await dbContext.SaveChangesAsync();

                return new Response<Transaction?>(data: null, message: "Transação removida com sucesso");
            }
            catch
            {
                return new Response<Transaction?>(data: null, code: 500, message: "Nào foi possível recuperar sua transação!");
            }
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                Transaction? transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id &&
                                                                        t.UserId == request.UserId);

                return transaction is null
                    ? new Response<Transaction?>(data: null, code: 404, message: "Transação não encontrada!")
                    : new Response<Transaction?>(data: transaction);
            }
            catch
            {
                return new Response<Transaction?>(data: null, code: 500, message: "Nào foi possível recuperar sua transação!");
            }
        }

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            try
            {
                // Coloca o valor na propriedade "StartDate e EndDate" se o valor do resultado dos métodos não for nulo
                request.StartDate ??= DateTime.Now.GetFirstDay();
                request.EndDate ??= DateTime.Now.GetLastDay();
            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(
                    data: null,
                    code: 500,
                    message: "Não foi possível determinar a data de início ou término");
            }

            try
            {
                IQueryable<Transaction> query = dbContext
                                                    .Transactions
                                                    .AsNoTracking()
                                                    .Where(t => t.PaidOrReceivedAt >= request.StartDate &&
                                                                t.PaidOrReceivedAt <= request.EndDate &&
                                                                t.UserId == request.UserId)
                                                    .OrderBy(t => t.PaidOrReceivedAt);

                int skip = ((request.PageNumber - 1) * request.PageSize);
                int take = request.PageSize;
                List<Transaction> transactions = await query.Skip(skip)
                                                            .Take(take)
                                                            .ToListAsync();

                int count = await query.CountAsync();

                return new PagedResponse<List<Transaction>?>(data: transactions,
                                                             totalCount: count,
                                                             currentPage: request.PageNumber,
                                                             pageSize: request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(
                    data: null,
                    code: 500,
                    message: "Não foi possível obter as transações!");
            }
        }
    }
}