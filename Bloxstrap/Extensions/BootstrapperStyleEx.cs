namespace Bloxstrap.Extensions
{
    static class BootstrapperStyleEx
    {
        public static IBootstrapperDialog GetNew(this BootstrapperStyle bootstrapperStyle) => Frontend.GetBootstrapperDialog(bootstrapperStyle);

        public static IReadOnlyCollection<BootstrapperStyle> Selections => new BootstrapperStyle[]
        {
            BootstrapperStyle.FluentDialog,
            BootstrapperStyle.FluentAeroDialog
        };
    }
}
