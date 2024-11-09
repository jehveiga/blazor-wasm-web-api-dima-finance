using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace Dima.Web.Pages.Identity
{
    public partial class RegisterPage : ComponentBase
    {
        #region Dependencies

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IAccountHandler Handler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        #endregion

        #region Properties

        public bool IsBusy { get; set; } = false;

        public RegisterRequest InputModel { get; set; } = new();

        #endregion

        #region Override
        protected override async Task OnInitializedAsync()
        {
            AuthenticationState authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
                NavigationManager.NavigateTo("/");
        }
        #endregion

        #region Methods

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                Response<string> result = await Handler.RegisterAsync(InputModel);

                if (result.IsSucess)
                {
                    Snackbar.Add(result.Message ?? string.Empty, Severity.Success);
                    NavigationManager.NavigateTo("/login");
                }
                else
                    Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message ?? string.Empty, Severity.Error);

            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
