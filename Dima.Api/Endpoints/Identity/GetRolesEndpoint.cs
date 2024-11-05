using Dima.Api.Common.Api;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Identity
{
    public class GetRolesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet(pattern: "/roles", handler: Handle)
               .RequireAuthorization();
        }

        private static IResult Handle(
            ClaimsPrincipal user)
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
        }
    }
}
