using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Data.Rankings;
using PBGame.Rulesets;
using PBGame.Graphics;
using PBFramework.Data.Bindables;
using PBFramework.Storages;

namespace PBGame.Configurations
{
    public class GameConfiguration : IGameConfiguration {

        public event Action<IGameConfiguration> OnLoad;

        private const string ConfigName = "game-configuration";

        private PrefStorage storage;

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
        public ProxyBindable<float> BackgroundDim { get; private set; }

        // ============================================================
        // Sound settings
        // ============================================================
        public ProxyBindable<float> MasterVolume { get; private set; }
        public ProxyBindable<float> MusicVolume { get; private set; }
        public ProxyBindable<float> HitsoundVolume { get; private set; }
        public ProxyBindable<float> EffectVolume { get; private set; }
        public ProxyBindable<int> GlobalOffset { get; private set; }
        public ProxyBindable<bool> UseBeatmapHitsounds { get; private set; }


        public GameConfiguration()
        {
            RulesetMode = InitEnumBindable(nameof(RulesetMode), GameModes.BeatsStandard);
            MapsetSort = InitEnumBindable(nameof(MapsetSort), MapsetSorts.Title);
            RankDisplay = InitEnumBindable(nameof(RankDisplay), RankDisplayTypes.Local);
            Username = InitStringBindable(nameof(Username), "");
            Password = InitStringBindable(nameof(Password), "");
            SaveCredentials = InitBoolBindable(nameof(SaveCredentials), false);

            PreferUnicode = InitBoolBindable(nameof(PreferUnicode), false);

            ShowFps = InitBoolBindable(nameof(ShowFps), false);
            UseBlurShader = InitBoolBindable(nameof(UseBlurShader), false);
            ResolutionQuality = InitEnumBindable(nameof(ResolutionQuality), ResolutionType.Best);

            ShowStoryboard = InitBoolBindable(nameof(ShowStoryboard), false);
            ShowVideo = InitBoolBindable(nameof(ShowVideo), false);
            UseBeatmapSkins = InitBoolBindable(nameof(UseBeatmapSkins), false);
            BackgroundDim = InitFloatBindable(nameof(BackgroundDim), 0.5f);

            MasterVolume = InitFloatBindable(nameof(MasterVolume), 1f);
            MusicVolume = InitFloatBindable(nameof(MusicVolume), 1f);
            HitsoundVolume = InitFloatBindable(nameof(HitsoundVolume), 1f);
            EffectVolume = InitFloatBindable(nameof(EffectVolume), 1f);
            GlobalOffset = InitIntBindable(nameof(GlobalOffset), 0);
            UseBeatmapHitsounds = InitBoolBindable(nameof(UseBeatmapHitsounds), true);
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
            return new ProxyBindable<T>(
                () => storage.GetEnum<T>(propertyName, defaultValue),
                (value) => storage.SetEnum(propertyName, value)
            );
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a string for type T.
        /// </summary>
        private ProxyBindable<string> InitStringBindable(string propertyName, string defaultValue)
        {
            return new ProxyBindable<string>(
                () => storage.GetString(propertyName, defaultValue),
                (value) => storage.SetString(propertyName, value)
            );
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a bool for type T.
        /// </summary>
        private ProxyBindable<bool> InitBoolBindable(string propertyName, bool defaultValue)
        {
            return new ProxyBindable<bool>(
                () => storage.GetBool(propertyName, defaultValue),
                (value) => storage.SetBool(propertyName, value)
            );
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a float for type T.
        /// </summary>
        private ProxyBindable<float> InitFloatBindable(string propertyName, float defaultValue)
        {
            return new ProxyBindable<float>(
                () => storage.GetFloat(propertyName, defaultValue),
                (value) => storage.SetFloat(propertyName, value)
            );
        }

        /// <summary>
        /// Instantiates a new proxy bindable, assuming a int for type T.
        /// </summary>
        private ProxyBindable<int> InitIntBindable(string propertyName, int defaultValue)
        {
            return new ProxyBindable<int>(
                () => storage.GetInt(propertyName, defaultValue),
                (value) => storage.SetInt(propertyName, value)
            );
        }
    }
}