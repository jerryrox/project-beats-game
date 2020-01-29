using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Animations;
using PBFramework.UI.Navigations;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Overlays
{
    public class BaseOverlay : UguiNavigationView, INavigationView {

        public override HideActions HideAction => HideActions.Recycle;

        [ReceivesDependency]
        protected IAnimePreset AniPreset { get; set; }


        protected override IAnime CreateShowAnime() => AniPreset?.GetDefaultScreenShow(this);

        protected override IAnime CreateHideAnime() => AniPreset?.GetDefaultScreenHide(this);
    }
}