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

        public ICommand ApplyPotatoPresetCommand => new RelayCommand(ApplyPotatoPreset);

        public ICommand ApplyPerformancePresetCommand => new RelayCommand(ApplyPerformancePreset);

        public ICommand ApplyBalancedPresetCommand => new RelayCommand(ApplyBalancedPreset);

        public bool UseFastFlagManager
        {
            get => App.Settings.Prop.UseFastFlagManager;
            set => App.Settings.Prop.UseFastFlagManager = value;
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

        public string TargetFps
        {
            get => App.FastFlags.GetPreset("Rendering.TargetFps") ?? "";
            set
            {
                App.FastFlags.SetPreset("Rendering.TargetFps", string.IsNullOrWhiteSpace(value) ? null : value);
                OnPropertyChanged(nameof(TargetFps));
            }
        }

        public bool UnlockFpsCap
        {
            get => App.FastFlags.GetPreset("Rendering.UnlockFpsCap") == "False";
            set
            {
                App.FastFlags.SetPreset("Rendering.UnlockFpsCap", value ? "False" : null);
                OnPropertyChanged(nameof(UnlockFpsCap));
            }
        }

        public bool DisablePostFx
        {
            get => App.FastFlags.GetPreset("Rendering.DisablePostFx") == "True";
            set
            {
                App.FastFlags.SetPreset("Rendering.DisablePostFx", value ? "True" : null);
                OnPropertyChanged(nameof(DisablePostFx));
            }
        }

        public bool DisableShadows
        {
            get => App.FastFlags.GetPreset("Rendering.Shadows") == "0";
            set
            {
                App.FastFlags.SetPreset("Rendering.Shadows", value ? 0 : null);
                OnPropertyChanged(nameof(DisableShadows));
            }
        }

        public bool DisableSSAO
        {
            get => App.FastFlags.GetPreset("Rendering.SSAO") == "True";
            set
            {
                App.FastFlags.SetPreset("Rendering.SSAO", value ? "True" : null);
                OnPropertyChanged(nameof(DisableSSAO));
            }
        }

        public bool LowTextures
        {
            get => App.FastFlags.GetPreset("Rendering.TexOverride") == "True";
            set
            {
                App.FastFlags.SetPreset("Rendering.TexOverride", value ? "True" : null);
                App.FastFlags.SetPreset("Rendering.TexLevel", value ? 1 : null);
                OnPropertyChanged(nameof(LowTextures));
            }
        }

        public bool DisableBlur
        {
            get => App.FastFlags.GetPreset("Rendering.GuiBlur") == "0";
            set
            {
                App.FastFlags.SetPreset("Rendering.GuiBlur", value ? 0 : null);
                OnPropertyChanged(nameof(DisableBlur));
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

        public bool LowLights
        {
            get => App.FastFlags.GetPreset("Lighting.UpdatesMax") == "1";
            set
            {
                App.FastFlags.SetPreset("Lighting.UpdatesMax", value ? 1 : null);
                App.FastFlags.SetPreset("Lighting.UpdatesMin", value ? 1 : null);
                OnPropertyChanged(nameof(LowLights));
            }
        }

        public bool DisableParticles
        {
            get => App.FastFlags.GetPreset("Effects.Particles") == "0";
            set
            {
                App.FastFlags.SetPreset("Effects.Particles", value ? 0 : null);
                OnPropertyChanged(nameof(DisableParticles));
            }
        }

        public bool DisableGrassStrands
        {
            get => App.FastFlags.GetPreset("Effects.GrassStrands") == "0";
            set
            {
                App.FastFlags.SetPreset("Effects.GrassStrands", value ? 0 : null);
                OnPropertyChanged(nameof(DisableGrassStrands));
            }
        }

        public bool SkyGray
        {
            get => App.FastFlags.GetPreset("Rendering.SkyGray") == "True";
            set
            {
                App.FastFlags.SetPreset("Rendering.SkyGray", value ? "True" : null);
                OnPropertyChanged(nameof(SkyGray));
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

        public bool PauseVoxelizer
        {
            get => App.FastFlags.GetPreset("Rendering.PauseVoxelizer") == "True";
            set
            {
                App.FastFlags.SetPreset("Rendering.PauseVoxelizer", value ? "True" : null);
                OnPropertyChanged(nameof(PauseVoxelizer));
            }
        }

        private void ApplyPotatoPreset()
        {
            App.Settings.Prop.UseFastFlagManager = true;
            TargetFps = "9999";
            UnlockFpsCap = true;
            DisablePostFx = true;
            DisableShadows = true;
            DisableSSAO = true;
            LowTextures = true;
            DisableBlur = true;
            ShowFps = true;
            LowLights = true;
            DisableParticles = true;
            DisableGrass = true;
            DisableGrassStrands = true;
            SkyGray = true;
            PauseVoxelizer = true;
            FixDisplayScaling = true;
            SelectedMSAALevel = MSAAMode.x0;
            FRMQualityOverrideEnabled = true;
            FRMQualityOverride = 1;
            MeshQualityEnabled = true;
            MeshQuality = 0;
            RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ApplyPerformancePreset()
        {
            App.Settings.Prop.UseFastFlagManager = true;
            TargetFps = "240";
            UnlockFpsCap = true;
            DisablePostFx = true;
            DisableShadows = true;
            DisableSSAO = true;
            LowTextures = true;
            DisableBlur = true;
            ShowFps = false;
            LowLights = true;
            DisableParticles = false;
            DisableGrass = true;
            DisableGrassStrands = true;
            SkyGray = false;
            PauseVoxelizer = true;
            FixDisplayScaling = true;
            SelectedMSAALevel = MSAAMode.x0;
            FRMQualityOverrideEnabled = true;
            FRMQualityOverride = 4;
            MeshQualityEnabled = true;
            MeshQuality = 1;
            RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ApplyBalancedPreset()
        {
            App.Settings.Prop.UseFastFlagManager = true;
            TargetFps = "144";
            UnlockFpsCap = true;
            DisablePostFx = false;
            DisableShadows = false;
            DisableSSAO = false;
            LowTextures = false;
            DisableBlur = true;
            ShowFps = false;
            LowLights = false;
            DisableParticles = false;
            DisableGrass = false;
            DisableGrassStrands = false;
            SkyGray = false;
            PauseVoxelizer = false;
            FixDisplayScaling = true;
            SelectedMSAALevel = MSAAMode.Default;
            FRMQualityOverrideEnabled = true;
            FRMQualityOverride = 8;
            MeshQualityEnabled = false;
            RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
