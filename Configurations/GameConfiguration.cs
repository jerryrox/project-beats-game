using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.UI.Navigations.Screens.Songs;
using PBFramework.Data.Bindables;
using PBFramework.Storages;

namespace PBGame.Configurations
{
    public class GameConfiguration : IGameConfiguration {

        private PrefStorage storage;


        public ProxyBindable<GameModes> RulesetMode { get; private set; }

        public ProxyBindable<MapsetSorts> MapsetSort { get; private set; }

        public ProxyBindable<MapDetailTabs> MapDetailTab { get; private set; }


        public ProxyBindable<string> Username { get; private set; }

        public ProxyBindable<string> Password { get; private set; }

        public ProxyBindable<bool> SaveCredentials { get; private set; }


        public ProxyBindable<bool> PreferUnicode { get; private set; }


        public ProxyBindable<float> MasterVolume { get; private set; }

        public ProxyBindable<float> MusicVolume { get; private set; }

        public ProxyBindable<float> HitsoundVolume { get; private set; }

        public ProxyBindable<float> EffectVolume { get; private set; }

        public ProxyBindable<int> GlobalOffset { get; private set; }

        public ProxyBindable<bool> UseBeatmapHitsounds { get; private set; }


        public ProxyBindable<bool> ShowFps { get; private set; }

        public ProxyBindable<bool> ShowStoryboard { get; private set; }

        public ProxyBindable<bool> ShowVideo { get; private set; }

        public ProxyBindable<bool> UseBeatmapSkins { get; private set; }

        public ProxyBindable<bool> UseBlurShader { get; private set; }

        public ProxyBindable<float> ResolutionQuality { get; private set; }


        public ProxyBindable<float> BackgroundDim { get; private set; }


        public GameConfiguration()
        {
            RulesetMode = InitEnumBindable(nameof(RulesetMode), GameModes.BeatsStandard);
            MapsetSort = InitEnumBindable(nameof(MapsetSort), MapsetSorts.Title);
            MapDetailTab = InitEnumBindable(nameof(MapDetailTab), MapDetailTabs.Details);

            Username = InitStringBindable(nameof(Username), "");
            Password = InitStringBindable(nameof(Password), "");
            SaveCredentials = InitBoolBindable(nameof(SaveCredentials), false);

            PreferUnicode = InitBoolBindable(nameof(PreferUnicode), false);

            MasterVolume = InitFloatBindable(nameof(MasterVolume), 1f);
            MusicVolume = InitFloatBindable(nameof(MusicVolume), 1f);
            HitsoundVolume = InitFloatBindable(nameof(HitsoundVolume), 1f);
            EffectVolume = InitFloatBindable(nameof(EffectVolume), 1f);
            GlobalOffset = InitIntBindable(nameof(GlobalOffset), 0);
            UseBeatmapHitsounds = InitBoolBindable(nameof(UseBeatmapHitsounds), true);

            ShowFps = InitBoolBindable(nameof(ShowFps), false);
            ShowStoryboard = InitBoolBindable(nameof(ShowStoryboard), false);
            ShowVideo = InitBoolBindable(nameof(ShowVideo), false);
            UseBeatmapSkins = InitBoolBindable(nameof(UseBeatmapSkins), false);
            UseBlurShader = InitBoolBindable(nameof(UseBlurShader), false);
            ResolutionQuality = InitFloatBindable(nameof(ResolutionQuality), 1f);
            
            BackgroundDim = InitFloatBindable(nameof(BackgroundDim), 0.5f);
        }

        public void Load()
        {
            storage = new PrefStorage("game-configuration");
        }

        public void Save()
        {
            storage.Save();
        }

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