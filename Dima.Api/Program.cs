using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Container de serviços
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction: x => x.CustomSchemaIds(n => n.FullName));

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

#endregion

WebApplication app = builder.Build();

#region Pipeline da Requisição

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(pattern: "/v1/categories",
    handler: async ([FromServices] ICategoryHandler handler) =>
    {
        GetAllCategoriesRequest request = new()
        {

            UserId = "teste2@veiga.io"
        };

        return await handler.GetAllAsync(request);
    })
.WithName("Categories: Get All")
.WithSummary("Retorna todas as categorias categoria de um usuário")
.Produces<PagedResponse<List<Category>?>>();

app.MapGet(pattern: "/v1/categories/{id}",
    handler: async (long id,
                    [FromServices] ICategoryHandler handler) =>
    {
        GetCategoryByIdRequest request = new()
        {
            Id = id,
            UserId = "teste2@veiga.io"
        };

        return await handler.GetByIdAsync(request);
    })
.WithName("Categories: Get by Id")
.WithSummary("Retorna uma categoria")
.Produces<Response<Category?>>();

app.MapPost(pattern: "/v1/categories",
    handler: async ([FromBody] CreateCategoryRequest request,
                    [FromServices] ICategoryHandler handler) =>
{
    return await handler.CreateAsync(request);
})
.WithName("Categories: Create")
.WithSummary("Cria uma nova categoria")
.Produces<Response<Category?>>();

app.MapPut(pattern: "/v1/categories/{id}",
    handler: async (long id,
                    [FromBody] UpdateCategoryRequest request,
                    [FromServices] ICategoryHandler handler) =>
    {
        request.Id = id;
        return await handler.UpdateAsync(request);
    })
.WithName("Categories: Update")
.WithSummary("Altera uma categoria")
.Produces<Response<Category?>>();

app.MapDelete(pattern: "/v1/categories/{id}",
    handler: async (long id,
                    [FromBody] DeleteCategoryRequest request,
                    [FromServices] ICategoryHandler handler) =>
    {
        request.Id = id;
        return await handler.DeleteAsync(request);
    })
.WithName("Categories: Delete")
.WithSummary("Exclui uma categoria")
.Produces<Response<Category?>>();

#endregion

await app.RunAsync();