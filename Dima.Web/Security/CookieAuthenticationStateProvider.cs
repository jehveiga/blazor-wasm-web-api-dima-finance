using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) :
    AuthenticationStateProvider,
    ICookieAuthenticationStateProvider
{
    private bool _isAuthenticated = false;
    private readonly HttpClient _client = clientFactory.CreateClient(ConfigurationHelpers.HTTPCLIENT_NAME);

    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _isAuthenticated;
    }

    public void NotifyAuthenticationStateChanged()
        => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _isAuthenticated = false;
        ClaimsPrincipal user = new(new ClaimsIdentity());

        User? userInfo = await GetUser();
        if (userInfo is null)
            return new AuthenticationState(user);

        List<Claim> claims = await GetClaims(userInfo);

        ClaimsIdentity id = new(claims, nameof(CookieAuthenticationStateProvider));
        user = new ClaimsPrincipal(id);

        _isAuthenticated = true;
        return new AuthenticationState(user);
    }

    private async Task<User?> GetUser()
    {
        try
        {
            return await _client.GetFromJsonAsync<User?>("v1/identity/manage/info");
        }
        catch
        {
            return null;
        }
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email),
            .. user.Claims.Where(x =>
                    x.Key != ClaimTypes.Name &&
                    x.Key != ClaimTypes.Email)
                .Select(x
                    => new Claim(x.Key, x.Value))
,
        ];

        RoleClaim[]? roles;
        try
        {
            roles = await _client.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
        }
        catch
        {
            return claims;
        }

        claims.AddRange(from RoleClaim role in roles ?? []
                        where !string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value)
                        select new Claim(role.Type!, role.Value!, role.ValueType, role.Issuer, role.OriginalIssuer));
        return claims;
    }
}