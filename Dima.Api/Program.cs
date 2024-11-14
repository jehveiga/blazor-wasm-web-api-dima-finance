using Dima.Api;
using Dima.Api.Common.Api;
using Dima.Api.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Container de servi�os

// Add services to the container.
builder.AddConfiguration();
builder.Services.AddProblemDetails();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

#endregion Container de servi�os

WebApplication app = builder.Build();

#region Pipeline da Requisi��o

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors(ApiConfiguration.CORS_POLICY_NAME);
app.UseSecurity();
app.MapEndpoints();
app.UseHttpsRedirection();

#endregion Pipeline da Requisi��o

await app.RunAsync();