using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.Background;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class BackgroundModel : BaseModel {

        private Bindable<BackgroundType> bgType = new Bindable<BackgroundType>();


        /// <summary>
        /// Returns the type of background variant suitable for current screen state.
        /// </summary>
        public IReadOnlyBindable<BackgroundType> BgType => bgType;

        /// <summary>
        /// Returns the map background loaded for current map.
        /// </summary>
        public IReadOnlyBindable<IMapBackground> Background => MapSelection.Background;

        /// <summary>
        /// Returns the current screen the user is in.
        /// </summary>
        public IReadOnlyBindable<INavigationView> CurrentScreen => ScreenNavigator.CurrentScreen;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        protected override void OnPreShow()
        {
            base.OnPreShow();

            CurrentScreen.BindAndTrigger(OnScreenChange);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            CurrentScreen.OnNewValue -= OnScreenChange;
        }

        /// <summary>
        /// Event called on current screen change.
        /// </summary>
        private void OnScreenChange(INavigationView screen)
        {
            if(screen is DownloadScreen)
                bgType.Value = BackgroundType.Empty;
            else
                bgType.Value = BackgroundType.Image;
        }
    }
}