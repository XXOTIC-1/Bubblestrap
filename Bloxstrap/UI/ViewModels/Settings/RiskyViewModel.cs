namespace Bloxstrap.UI.ViewModels.Settings
{
    /// <summary>
    /// Flags that change what the client shows you rather than how fast it runs. Roblox knows about
    /// these, so everything here stays disabled until the user accepts the warning.
    /// </summary>
    public class RiskyViewModel : NotifyPropertyChangedViewModel
    {
        public bool Acknowledged
        {
            get => App.Settings.Prop.RiskyFlagsAcknowledged;
            set
            {
                App.Settings.Prop.RiskyFlagsAcknowledged = value;

                if (!value)
                    ClearAll();

                OnPropertyChanged(nameof(Acknowledged));
            }
        }

        /// <summary>
        /// The usual fullbright pack: gray sky plus a paused voxelizer removes baked shadows and
        /// flattens lighting. Shadow intensity is not included because that flag no longer resolves.
        /// </summary>
        public bool Fullbright
        {
            get => App.FastFlags.GetPreset("Risky.SkyGray") == "True"
                && App.FastFlags.GetPreset("Risky.PauseVoxelizer") == "True";
            set
            {
                App.FastFlags.SetPreset("Risky.SkyGray", value ? "True" : null);
                App.FastFlags.SetPreset("Risky.PauseVoxelizer", value ? "True" : null);
                OnPropertyChanged(nameof(Fullbright));
            }
        }

        public bool Wireframe
        {
            get => App.FastFlags.GetPreset("Risky.Wireframe") == "True";
            set
            {
                App.FastFlags.SetPreset("Risky.Wireframe", value ? "True" : null);
                OnPropertyChanged(nameof(Wireframe));
            }
        }

        public bool SkipMeshVoxelizer
        {
            get => App.FastFlags.GetPreset("Risky.SkipMeshVoxelizer") == "True";
            set
            {
                App.FastFlags.SetPreset("Risky.SkipMeshVoxelizer", value ? "True" : null);
                OnPropertyChanged(nameof(SkipMeshVoxelizer));
            }
        }

        private void ClearAll()
        {
            Fullbright = false;
            Wireframe = false;
            SkipMeshVoxelizer = false;
        }
    }
}
