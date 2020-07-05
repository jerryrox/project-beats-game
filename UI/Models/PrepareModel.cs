using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class PrepareModel : BaseModel {

        private BindableBool isDetailedMode = new BindableBool(false);


        /// <summary>
        /// Returns whether the map information should be displayed as detailed mode.
        /// </summary>
        public IReadOnlyBindable<bool> IsDetailedMode => isDetailedMode;

        /// <summary>
        /// Returns the currently selected map.
        /// </summary>
        public IReadOnlyBindable<IPlayableMap> SelectedMap => MapSelection.Map;

        /// <summary>
        /// Returns whether unicode is preferred.
        /// </summary>
        public IReadOnlyBindable<bool> PreferUnicode => GameConfiguration.PreferUnicode;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            
        }

        /// <summary>
        /// Toggles between detailed/brief information display mode.
        /// </summary>
        public void ToggleDetailedMode() => SetDetailedMode(!isDetailedMode.Value);

        /// <summary>
        /// Sets the detailed/brief display mode.
        /// </summary>
        public void SetDetailedMode(bool isDetailed) => isDetailedMode.Value = isDetailed;

        protected override void OnPreShow()
        {
            base.OnPreShow();
            SetDetailedMode(false);
        }
    }
}