using Bloxstrap.UI.ViewModels.Settings;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for RiskyPage.xaml
    /// </summary>
    public partial class RiskyPage
    {
        public RiskyPage()
        {
            DataContext = new RiskyViewModel();
            InitializeComponent();
        }
    }
}
