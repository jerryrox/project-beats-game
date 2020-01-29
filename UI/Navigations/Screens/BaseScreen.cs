using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Animations;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Utils;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public abstract class BaseScreen : UguiNavigationView, IBaseScreen {

        public override HideActions HideAction => HideActions.Recycle;

        [ReceivesDependency]
        protected IAnimePreset AniPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
        }

        /// <summary>
        /// Creates a new instance of the screen show animation.
        /// </summary>
        protected override IAnime CreateShowAnime() => AniPreset?.GetDefaultScreenShow(this);

        /// <summary>
        /// Creates a new instance of the screen hide animation.
        /// </summary>
        protected override IAnime CreateHideAnime() => AniPreset?.GetDefaultScreenHide(this);
    }
}