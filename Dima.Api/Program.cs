using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Container de servi�os
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen(setupAction: x => x.CustomSchemaIds(n => n.FullName));

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorization();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Adicionando o servi�o para trabalhar com identity
builder.Services.AddIdentityCore<User>() // Adiciona ao servi�o do Identity a classe customizada que foi criada de User
                .AddRoles<IdentityRole<long>>() // Adiciona ao servi�o do Identity a classe customizada que foi criada de Role
                .AddEntityFrameworkStores<AppDbContext>() // Adicionando o servi�o do banco que ser� usado para cria��o das tabelas do Identity
                .AddApiEndpoints(); // Cria automaticamente os endpoint para o servi�o de autentica��o

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

#endregion

WebApplication app = builder.Build();

#region Pipeline da Requisi��o

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