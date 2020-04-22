using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Data.Rankings;
using PBGame.Rulesets;
using PBGame.Graphics;
using PBGame.Configurations.Settings;
using PBFramework.Data.Bindables;
using PBFramework.Storages;

namespace PBGame.Configurations
{
    public class GameConfiguration : IGameConfiguration {

        public event Action<IGameConfiguration> OnLoad;

        private const string ConfigName = "game-configuration";

        private PrefStorage storage;

        /// <summary>
        /// List of all settings stored as the bare minimum interface.
        /// </summary>
        private List<IBindable> allSettings = new List<IBindable>();


        public ISettingsData Settings { get; private set; }

        // ============================================================
        // Internal settings
        // ============================================================
        public ProxyBindable<GameModes> RulesetMode { get; private set; }
        public ProxyBindable<MapsetSorts> MapsetSort { get; private set; }
        public ProxyBindable<RankDisplayTypes> RankDisplay { get; private set; }
        public ProxyBindable<string> Username { get; private set; }
        public ProxyBindable<string> Password { get; private set; }
        public ProxyBindable<bool> SaveCredentials { get; private set; }

        // ============================================================
        // General settings
        // ============================================================
        public ProxyBindable<bool> PreferUnicode { get; private set; }

        // ============================================================
        // Performance settings
        // ============================================================
        public ProxyBindable<bool> ShowFps { get; private set; }
        public ProxyBindable<bool> UseBlurShader { get; private set; }
        public ProxyBindable<ResolutionType> ResolutionQuality { get; private set; }

        // ============================================================
        // Gameplay settings
        // ============================================================
        public ProxyBindable<bool> ShowStoryboard { get; private set; }
        public ProxyBindable<bool> ShowVideo { get; private set; }
        public ProxyBindable<bool> UseBeatmapSkins { get; private set; }
        public ProxyBindableFloat BackgroundDim { get; private set; }

        // ============================================================
        // Sound settings
        // ============================================================
        public ProxyBindableFloat MasterVolume { get; private set; }
        public ProxyBindableFloat MusicVolume { get; private set; }
        public ProxyBindableFloat HitsoundVolume { get; private set; }
        public ProxyBindableFloat EffectVolume { get; private set; }
        public ProxyBindableInt GlobalOffset { get; private set; }
        public ProxyBindable<bool> UseBeatmapHitsounds { get; private set; }


        public GameConfiguration()
        {
            Settings = new SettingsData();

            RulesetMode = InitEnumBindable(nameof(RulesetMode), GameModes.BeatsStandard);
            MapsetSort = InitEnumBindable(nameof(MapsetSort), MapsetSorts.Title);
            RankDisplay = InitEnumBindable(nameof(RankDisplay), RankDisplayTypes.Local);
            Username = InitStringBindable(nameof(Username), "");
            Password = InitStringBindable(nameof(Password), "");
            SaveCredentials = InitBoolBindable(nameof(SaveCredentials), false);

            // General settings
            SettingsTab generalTab = Settings.AddTabData(new SettingsTab("General", "icon-settings"));
            {
                generalTab.AddEntry(new SettingsEntryBool("Prefer unicode", PreferUnicode = InitBoolBindable(nameof(PreferUnicode), false)));
            }

            // Performance settings
            SettingsTab performanceTab = Settings.AddTabData(new SettingsTab("Performance", "icon-performance"));
            {
                performanceTab.AddEntry(new SettingsEntryBool("Show FPS", ShowFps = InitBoolBindable(nameof(ShowFps), false)));
                performanceTab.AddEntry(new SettingsEntryBool("Use Blur", UseBlurShader = InitBoolBindable(nameof(UseBlurShader), false)));
                performanceTab.AddEntry(new SettingsEntryEnum<ResolutionType>("Resolution Quality", ResolutionQuality = InitEnumBindable(nameof(ResolutionQuality), ResolutionType.Best)));
            }

            // Gameplay settings
            SettingsTab gameplayTab = Settings.AddTabData(new SettingsTab("Gameplay", "icon-game"));
            {
                gameplayTab.AddEntry(new SettingsEntryBool("Use Storyboard", ShowStoryboard = InitBoolBindable(nameof(ShowStoryboard), false)));
                gameplayTab.AddEntry(new SettingsEntryBool("Use Video", ShowVideo = InitBoolBindable(nameof(ShowVideo), false)));
                gameplayTab.AddEntry(new SettingsEntryBool("Use Map Skins", UseBeatmapSkins = InitBoolBindable(nameof(UseBeatmapSkins), false)));
                gameplayTab.AddEntry(new SettingsEntryFloat("Background Dim", BackgroundDim = InitFloatBindable(nameof(BackgroundDim), 0.5f, 0f, 1f))
                {
                    Formatter = "P0"
                });
            }

            // Sound settings
            SettingsTab soundTab = Settings.AddTabData(new SettingsTab("Sound", "icon-sound"));
            {
                soundTab.AddEntry(new SettingsEntryFloat("Master Volume", MasterVolume = InitFloatBindable(nameof(MasterVolume), 1f, 0f, 1f))
                {
                    Formatter = "P0"
                });
                soundTab.AddEntry(new SettingsEntryFloat("Music Volume", MusicVolume = InitFloatBindable(nameof(MusicVolume), 1f, 0f, 1f))
                {
                    Formatter = "P0"
                });
                soundTab.AddEntry(new SettingsEntryFloat("Hitsound Volume", HitsoundVolume = InitFloatBindable(nameof(HitsoundVolume), 1f, 0f, 1f))
                {
                    Formatter = "P0"
                });
                soundTab.AddEntry(new SettingsEntryFloat("Effect Volume", EffectVolume = InitFloatBindable(nameof(EffectVolume), 1f, 0f, 1f))
                {
                    Formatter = "P0"
                });
                soundTab.AddEntry(new SettingsEntryInt("Global Offset", GlobalOffset = InitIntBindable(nameof(GlobalOffset), 0, -100, 100)));
                soundTab.AddEntry(new SettingsEntryBool("Use Map Hitsounds", UseBeatmapHitsounds = InitBoolBindable(nameof(UseBeatmapHitsounds), true)));
            }

            // Trigger change for all configurations on load.
            OnLoad += delegate
            {
                foreach(var entry in allSettings)
                    entry.Trigger();
            };
        }

        public void Load()
        {
            storage = new PrefStorage(ConfigName);
            OnLoad?.Invoke(this);
        }

        public void Save()
        {
            storage.Save();
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("PBGame/ClearGameConfiguration")]
        private static void ClearGameConfiguration()
        {
            UnityEngine.PlayerPrefs.DeleteKey(ConfigName);
            UnityEngine.PlayerPrefs.Save();
            UnityEngine.Debug.Log("Deleted game configuration.");
        }
#endif

        /// <summary>
        /// Instantiates a new proxy bindable, assuming an enum for type T.
        /// </summary>
        private ProxyBindable<T> InitEnumBindable<T>(string propertyName, T defaultValue)
            where T : struct
        {
            var bindable = new ProxyBindable<T>(
                () => storage.GetEnum<T>(propertyName, defaultValue),
                (value) => storage.SetEnum(propertyName, value)
            );
            allSettings.Add(bindable);
            return bindable;
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a string for type T.
        /// </summary>
        private ProxyBindable<string> InitStringBindable(string propertyName, string defaultValue)
        {
            var bindable = new ProxyBindable<string>(
                () => storage.GetString(propertyName, defaultValue),
                (value) => storage.SetString(propertyName, value)
            );
            allSettings.Add(bindable);
            return bindable;
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a bool for type T.
        /// </summary>
        private ProxyBindable<bool> InitBoolBindable(string propertyName, bool defaultValue)
        {
            var bindable = new ProxyBindable<bool>(
                () => storage.GetBool(propertyName, defaultValue),
                (value) => storage.SetBool(propertyName, value)
            );
            allSettings.Add(bindable);
            return bindable;
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a float for type T.
        /// </summary>
        private ProxyBindableFloat InitFloatBindable(string propertyName, float defaultValue, float minValue, float maxValue)
        {
            var bindable = new ProxyBindableFloat(
                () => storage == null ? defaultValue : storage.GetFloat(propertyName, defaultValue),
                (value) => storage.SetFloat(propertyName, value),
                minValue,
                maxValue
            );
            allSettings.Add(bindable);
            return bindable;
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a int for type T.
        /// </summary>
        private ProxyBindableInt InitIntBindable(string propertyName, int defaultValue, int minValue, int maxValue)
        {
            var bindable = new ProxyBindableInt(
                () => storage == null ? defaultValue : storage.GetInt(propertyName, defaultValue),
                (value) => storage.SetInt(propertyName, value),
                minValue,
                maxValue
            );
            allSettings.Add(bindable);
            return bindable;
        }
    }
}