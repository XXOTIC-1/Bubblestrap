using System.Collections.ObjectModel;

namespace Bloxstrap.Models.Persistable
{
    public class Settings
    {
        public bool AllowCookieAccess { get; set; } = false;

        // configuration
        public BootstrapperStyle BootstrapperStyle { get; set; } = BootstrapperStyle.FluentAeroDialog;
        public BootstrapperIcon BootstrapperIcon { get; set; } = BootstrapperIcon.IconBubblestrap;
        public string BootstrapperTitle { get; set; } = App.ProjectName;
        public string BootstrapperIconCustomLocation { get; set; } = "";
        public Theme Theme { get; set; } = Theme.Dark;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool DeveloperMode { get; set; } = false;
        public bool ForceLocalData { get; set; } = false;
        public bool CheckForUpdates { get; set; } = false;
        public bool AutoCloseCrashHandler { get; set; } = true;
        public bool HideBootstrapperInfo { get; set; } = true;
        public bool EnableMemoryTrimmer { get; set; } = true;
        public int MemoryTrimInterval { get; set; } = 10;
        public bool EnableMemoryThreshold { get; set; } = false;
        public int MemoryTrimThreshold { get; set; } = 0;
        public bool MultiInstanceLaunching { get; set; } = false;
        public bool ConfirmLaunches { get; set; } = false;
        public string Locale { get; set; } = "nil";
        public bool UseFastFlagManager { get; set; } = true;

        // Only flags on FastFlagManager.AllowedFlags get written to the client while this is on.
        // Off-list flags stay in the user's list but are skipped, so a pasted flag pack can't
        // quietly ship hundreds of unknown values.
        public bool StrictFlagAllowlist { get; set; } = true;

        // Set once the user accepts the warning on the Risky page. Gates every risky control.
        public bool RiskyFlagsAcknowledged { get; set; } = false;
        public bool WPFSoftwareRender { get; set; } = false;
        public bool UpdateRoblox { get; set; } = true;
        public bool SkipRobloxUpgrades { get; set; } = false;
        public bool UsePreviousVersion { get; set; } = false;
        public bool StaticDirectory { get; set; } = false;
        public string Channel { get; set; } = RobloxInterfaces.Deployment.DefaultChannel;
        public ChannelChangeMode ChannelChangeMode { get; set; } = ChannelChangeMode.Automatic;
        public string ChannelHash { get; set; } = "";
        public string DownloadingStringFormat { get; set; } = Strings.Bootstrapper_Status_Downloading + " {0} - {1}MB / {2}MB";
        public string? SelectedCustomTheme { get; set; } = null;
        public bool BackgroundUpdatesEnabled { get; set; } = false;
        public bool MinimizeToTray { get; set; } = false;
        public bool EnableTrayModal { get; set; } = false;
        public bool LaunchOnStartup { get; set; } = false;
        public string RobloxTheme { get; set; } = "Dark";
        public bool DebugDisableVersionPackageCleanup { get; set; } = false;
        public WebEnvironment WebEnvironment { get; set; } = WebEnvironment.Production;

        // integration configuration
        public CleanerOptions CleanerOptions { get; set; } = CleanerOptions.Never;
        public List<string> CleanerDirectories { get; set; } = new List<string>();

        // Hidden, not gone. These have no settings UI anymore, but Watcher, Bootstrapper and the
        // tray icon still branch on them. They are pinned to safe values rather than deleted so
        // the watcher keeps working and existing config files stay readable.
        public bool EnableActivityTracking { get; set; } = true;
        public bool ShowServerDetails { get; set; } = false;
        public ObservableCollection<CustomIntegration> CustomIntegrations { get; set; } = new();
        public bool UseDisableAppPatch { get; set; } = false;
    }
}