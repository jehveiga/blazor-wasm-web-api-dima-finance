using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(pattern: "/", handler: () => new { message = "Ok" });
app.MapEndpoints();
app.MapGroup(prefix: "v1/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapGroup(prefix: "v1/identity")
   .WithTags("Identity")
   .MapPost(pattern: "/logout", handler: async (
       SignInManager<User> signInManager) =>
   {
       await signInManager.SignOutAsync();
       return Results.Ok();
   })
   .RequireAuthorization();

app.MapGroup(prefix: "v1/identity")
   .WithTags("Identity")
   .MapPost(pattern: "/roles", handler: (
       ClaimsPrincipal user) =>
   {
       if (user.Identity is null || !user.Identity.IsAuthenticated)
           return Results.Unauthorized();

       ClaimsIdentity identity = (ClaimsIdentity)user.Identity;
       var roles = identity.FindAll(identity.RoleClaimType)
                                          .Select(c => new
                                          {
                                              c.Issuer,
                                              c.OriginalIssuer,
                                              c.Type,
                                              c.Value,
                                              c.ValueType
                                          });
       return TypedResults.Json(roles);
   })
   .RequireAuthorization();

#endregion

await app.RunAsync();