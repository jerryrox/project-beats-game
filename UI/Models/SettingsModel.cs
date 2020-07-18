using PBGame.Configurations;
using PBGame.Configurations.Settings;
using PBFramework.Data.Bindables;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class SettingsModel : BaseModel {

        private Bindable<ISettingsData> currentSettings = new Bindable<ISettingsData>();


        /// <summary>
        /// Returns the current settings data to be displayed.
        /// </summary>
        public IReadOnlyBindable<ISettingsData> CurrentSettings => currentSettings;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        /// <summary>
        /// Sets the settings data to visually represent.
        /// </summary>
        public void SetSettingsData(ISettingsData settings)
        {
            currentSettings.Value = settings;
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();
            SetSettingsData(GameConfiguration.Settings);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();
            GameConfiguration.Save();
        }
    }
}