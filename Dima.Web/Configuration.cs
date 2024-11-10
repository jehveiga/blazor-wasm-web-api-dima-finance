using MudBlazor;
using MudBlazor.Utilities;

namespace Dima.Web
{
    internal static class Configuration
    {
        public const string HTTPCLIENT_NAME = "dima";

        public static string BackendUrl { get; set; } = string.Empty;

        public readonly static MudTheme Theme = new()
        {
            Typography = new Typography
            {
                Default = new Default
                {
                    FontFamily = ["Raleway", "sans-serif"]
                }
            },
            PaletteLight = new PaletteLight
            {
                Primary = "#1EFA2D",
                Secondary = Colors.LightGreen.Darken3,
                Background = Colors.Gray.Lighten4,
                AppbarBackground = new MudColor("#1EFA2D"),
                AppbarText = Colors.Shades.Black,
                TextPrimary = Colors.Shades.Black,
                PrimaryContrastText = new MudColor("000000"),
                DrawerText = Colors.Shades.White,
                DrawerBackground = Colors.Green.Darken4
            },
            PaletteDark = new PaletteDark
            {
                Primary = Colors.LightGreen.Accent3,
                Secondary = Colors.LightGreen.Darken3,
                AppbarBackground = Colors.LightGreen.Accent3,
                AppbarText = Colors.Shades.Black,
                PrimaryContrastText = new MudColor("000000")
            }
        };
    }
}