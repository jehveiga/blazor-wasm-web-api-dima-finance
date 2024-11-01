using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;

namespace Dima.Api.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            RouteGroupBuilder endpoints = app.MapGroup(prefix: "");

            endpoints.MapGroup("v1/categories")
                .WithTags("Categories")
                // .RequireAuthorization()
                .MapEndpoint<CreateCategoryEndpoint>()
                .MapEndpoint<UpdateCategoryEndpoint>()
                .MapEndpoint<DeleteCategoryEndpoint>()
                .MapEndpoint<GetCategoryByIdEndpoint>()
                .MapEndpoint<GettAllCategoriesEndpoint>();
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
            where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);

            return app;
        }
    }
}
