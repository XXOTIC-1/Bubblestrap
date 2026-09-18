using Bloxstrap.Enums.FlagPresets;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Bloxstrap.UI.ViewModels.Settings
{
    public class FastFlagsViewModel : NotifyPropertyChangedViewModel
    {
        private Dictionary<string, object>? _preResetFlags;

        public event EventHandler? RequestPageReloadEvent;

        public event EventHandler? OpenFlagEditorEvent;

        private void OpenFastFlagEditor() => OpenFlagEditorEvent?.Invoke(this, EventArgs.Empty);

        public ICommand OpenFastFlagEditorCommand => new RelayCommand(OpenFastFlagEditor);

        public ICommand ApplyMaxFpsPresetCommand => new RelayCommand(ApplyMaxFpsPreset);

        public ICommand ApplyBalancedPresetCommand => new RelayCommand(ApplyBalancedPreset);

        public bool UseFastFlagManager
        {
            get => App.Settings.Prop.UseFastFlagManager;
            set => App.Settings.Prop.UseFastFlagManager = value;
        }

        public bool StrictFlagAllowlist
        {
            get => App.Settings.Prop.StrictFlagAllowlist;
            set => App.Settings.Prop.StrictFlagAllowlist = value;
        }

        public IReadOnlyDictionary<MSAAMode, string?> MSAALevels => FastFlagManager.MSAAModes;

        public MSAAMode SelectedMSAALevel
        {
            get => MSAALevels.FirstOrDefault(x => x.Value == App.FastFlags.GetPreset("Rendering.MSAA")).Key;
            set => App.FastFlags.SetPreset("Rendering.MSAA", MSAALevels[value]);
        }

        public IReadOnlyDictionary<RenderingMode, string> RenderingModes => FastFlagManager.RenderingModes;

        public RenderingMode SelectedRenderingMode
        {
            get => App.FastFlags.GetPresetEnum(RenderingModes, "Rendering.Mode", "True");
            set
            {
                RenderingMode[] DisableD3D11 = new RenderingMode[]
                {
                    RenderingMode.Vulkan,
                };

                App.FastFlags.SetPresetEnum("Rendering.Mode", value.ToString(), "True");
                App.FastFlags.SetPreset("Rendering.Mode.DisableD3D11", DisableD3D11.Contains(value) ? "True" : null);
            }
        }

        public bool FixDisplayScaling
        {
            get => App.FastFlags.GetPreset("Rendering.DisableScaling") == "True";
            set => App.FastFlags.SetPreset("Rendering.DisableScaling", value ? "True" : null);
        }

        private static readonly string[] LODLevels = { "L0", "L12", "L23", "L34" };

        /// <summary>
        /// The client's own framerate cap. This replaces the framerate box that used to live on the
        /// Global Settings page, so the cap stays reachable from the UI instead of only in JSON.
        /// </summary>
        public bool FpsCapEnabled
        {
            get => App.FastFlags.GetPreset("Rendering.TargetFps") != null;
            set
            {
                if (value)
                {
                    App.FastFlags.SetPreset("Rendering.TargetFps", 240);
                }
                else
                {
                    App.FastFlags.SetPreset("Rendering.TargetFps", null);
                    App.FastFlags.SetPreset("Rendering.UnlockFpsCap", null);
                }

                OnPropertyChanged(nameof(FpsCapEnabled));
                OnPropertyChanged(nameof(FpsCap));
            }
        }

        public int FpsCap
        {
            get => int.TryParse(App.FastFlags.GetPreset("Rendering.TargetFps"), out var x) ? x : 240;
            set
            {
                int clamped = Math.Clamp(value, 30, 1000);

                App.FastFlags.SetPreset("Rendering.TargetFps", clamped);

                // Roblox clamps the scheduler to 240 unless this is turned off, so anything above
                // 240 does nothing on its own.
                App.FastFlags.SetPreset("Rendering.UnlockFpsCap", clamped > 240 ? "False" : null);

                OnPropertyChanged(nameof(FpsCap));
                OnPropertyChanged(nameof(FpsCapEnabled));
            }
        }

        public bool ShowFps
        {
            get => App.FastFlags.GetPreset("Rendering.ShowFps") == "True";
            set
            {
                App.FastFlags.SetPreset("Rendering.ShowFps", value ? "True" : null);
                OnPropertyChanged(nameof(ShowFps));
            }
        }

        public bool LowTextures
        {
            get => App.FastFlags.GetPreset("Rendering.TexOverride") == "True";
            set
            {
                App.FastFlags.SetPreset("Rendering.TexOverride", value ? "True" : null);
                App.FastFlags.SetPreset("Rendering.TexLevel", value ? 0 : null);
                OnPropertyChanged(nameof(LowTextures));
            }
        }

        public bool DisableGrass
        {
            get => App.FastFlags.GetPreset("Rendering.Grass.Max") == "0";
            set
            {
                object? grassValue = value ? 0 : null;

                App.FastFlags.SetPreset("Rendering.Grass", grassValue);

                OnPropertyChanged(nameof(DisableGrass));
            }
        }

        public bool FRMQualityOverrideEnabled
        {
            get => App.FastFlags.GetPreset("Rendering.FRMQualityOverride") != null;
            set
            {
                if (value)
                    FRMQualityOverride = 21;
                else
                    App.FastFlags.SetPreset("Rendering.FRMQualityOverride", null);

                OnPropertyChanged(nameof(FRMQualityOverride));
                OnPropertyChanged(nameof(FRMQualityOverrideEnabled));
            }
        }

        public int FRMQualityOverride
        {
            get => int.TryParse(App.FastFlags.GetPreset("Rendering.FRMQualityOverride"), out var x) ? x : 21;
            set
            {
                App.FastFlags.SetPreset("Rendering.FRMQualityOverride", value);

                OnPropertyChanged(nameof(FRMQualityOverride));
            }
        }

        public bool MeshQualityEnabled
        {
            get => App.FastFlags.GetPreset("Geometry.MeshLOD.Static") != null;
            set
            {
                if (value)
                {
                    // we enable level 3 by default
                    MeshQuality = 3;
                }
                else
                {
                    foreach (string level in LODLevels)
                        App.FastFlags.SetPreset($"Geometry.MeshLOD.{level}", null);

                    App.FastFlags.SetPreset("Geometry.MeshLOD.Static", null);
                }

                OnPropertyChanged(nameof(MeshQualityEnabled));
            }
        }

        public int MeshQuality
        {
            get => int.TryParse(App.FastFlags.GetPreset("Geometry.MeshLOD.Static"), out var x) ? x : 0;
            set
            {
                int clamped = Math.Clamp(value, 0, LODLevels.Length - 1);

                for (int i = 0; i < LODLevels.Length; i++)
                {
                    int lodValue = (Math.Clamp(clamped - i, 0, 3) + 1) * 250;
                    string lodLevel = LODLevels[i];

                    App.FastFlags.SetPreset($"Geometry.MeshLOD.{lodLevel}", lodValue);
                }

                App.FastFlags.SetPreset("Geometry.MeshLOD.Static", clamped);
                OnPropertyChanged(nameof(MeshQuality));
                OnPropertyChanged(nameof(MeshQualityEnabled));
            }
        }

        public bool ResetConfiguration
        {
            get => _preResetFlags is not null;

            set
            {
                if (value)
                {
                    _preResetFlags = new(App.FastFlags.Prop);
                    App.FastFlags.Prop.Clear();
                }
                else
                {
                    App.FastFlags.Prop = _preResetFlags!;
                    _preResetFlags = null;
                }

                RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ApplyMaxFpsPreset()
        {
            App.Settings.Prop.UseFastFlagManager = true;

            FpsCap = 1000;
            ShowFps = true;
            LowTextures = true;
            DisableGrass = true;
            FixDisplayScaling = true;
            SelectedMSAALevel = MSAAMode.x0;
            FRMQualityOverrideEnabled = true;
            FRMQualityOverride = 1;
            MeshQualityEnabled = true;
            MeshQuality = 0;

            RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ApplyBalancedPreset()
        {
            App.Settings.Prop.UseFastFlagManager = true;

            FpsCap = 240;
            ShowFps = false;
            LowTextures = false;
            DisableGrass = false;
            FixDisplayScaling = true;
            SelectedMSAALevel = MSAAMode.Default;
            FRMQualityOverrideEnabled = false;
            MeshQualityEnabled = false;

            RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
