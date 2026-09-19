using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace Bloxstrap.UI.Appearance
{
    public static class TuffstrapTheme
    {
        /// <summary>Primary brand gold used for accents and primary actions.</summary>
        public static readonly Color AccentGold = Color.FromRgb(0xC9, 0xA2, 0x27);

        public static void ApplyAccent(IThemeService? themeService = null)
        {
            themeService ??= new ThemeService();
            themeService.SetAccent(AccentGold);
        }

        public static void ApplyDarkTheme(IThemeService themeService)
        {
            themeService.SetTheme(ThemeType.Dark);
            ApplyAccent(themeService);
        }
    }
}
