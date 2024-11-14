using Dima.Api.Common.Api;
using Dima.Core.Models.Account;
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
            IEnumerable<RoleClaim> roles = identity.FindAll(identity.RoleClaimType)
                                               .Select(c => new RoleClaim
                                               {
                                                   Issuer = c.Issuer,
                                                   OriginalIssuer = c.OriginalIssuer,
                                                   Type = c.Type,
                                                   Value = c.Value,
                                                   ValueType = c.ValueType
                                               });
            return TypedResults.Json(roles);
        }
    }
}