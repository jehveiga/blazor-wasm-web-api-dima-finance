using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Container de serviços
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
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

app.MapEndpoints();

#endregion

await app.RunAsync();