using Dima.Api.Data;
using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction: x => x.CustomSchemaIds(n => n.FullName));

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<Handler>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost(pattern: "/v1/categories", handler: (Request request, Handler handler) =>
{
    return handler.Handle(request);
})
.WithName("Categories: Create")
.WithSummary("Cria uma nova categoria")
.Produces<Response>();

app.Run();

// Request
public class Request
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

// Response
public class Response
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

// Handler
public class Handler(AppDbContext dbContext)
{
    public Response Handle(Request request)
    {
        Category category = new()
        {
            Title = request.Title,
            Description = request.Description
        };
        dbContext.Categories.Add(category);
        dbContext.SaveChanges();

        return new Response
        {
            Id = category.Id,
            Title = category.Title
        };
    }
}