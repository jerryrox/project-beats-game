using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Maps;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class SearchBar : GlowInput {

        [ReceivesDependency]
        private SongsModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            OnChanged += Model.ScheduleSearch;
            OnSubmitted += Model.ApplySearch;

            backgroundSprite.Color = new Color(1f, 1f, 1f, 0.0625f);
            backgroundSprite.Anchor = AnchorType.Fill;
            backgroundSprite.Offset = Offset.Zero;

            CreateIconSprite(spriteName: "icon-search");

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            Text = Model.LastSearchTerm;
        }
    }
}