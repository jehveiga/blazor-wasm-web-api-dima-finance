using Dima.Api.Common.Api;
using Dima.Api.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Container de serviços
// Add services to the container.
builder.AddConfiguration();
builder.Services.AddProblemDetails();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

#endregion

WebApplication app = builder.Build();

#region Pipeline da Requisição

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseSecurity();
app.MapEndpoints();
app.UseHttpsRedirection();

#endregion

await app.RunAsync();