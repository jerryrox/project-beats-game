using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare
{
    public class SongMeta : UguiObject {

        private ISprite gradient;
        private ILabel title;
        private ILabel artist;
        private ILabel creator;

        private IAnime fadeAni;

        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            gradient = CreateChild<UguiSprite>("gradient", 0);
            {
                gradient.Anchor = AnchorType.Fill;
                gradient.RawSize = Vector2.zero;
                gradient.SpriteName = "gradation-bottom";
                gradient.Color = Color.black;
            }
            title = CreateChild<Label>("title", 1);
            {
                title.Anchor = AnchorType.BottomStretch;
                title.RawWidth = -84f;
                title.Y = 56f;
                title.IsBold = true;
                title.Alignment = TextAnchor.MiddleLeft;
                title.FontSize = 26;
            }
            artist = CreateChild<Label>("artist", 2);
            {
                artist.Anchor = AnchorType.BottomStretch;
                artist.Pivot = PivotType.BottomLeft;
                artist.SetOffsetHorizontal(42f, 402f);
                artist.Y = 12f;
                artist.Height = 34f;
                artist.FontSize = 22;
                artist.Alignment = TextAnchor.MiddleLeft;
            }
            creator = CreateChild<Label>("creator", 3);
            {
                creator.Anchor = AnchorType.BottomStretch;
                creator.Pivot = PivotType.BottomRight;
                creator.Alignment = TextAnchor.MiddleRight;
                creator.FontSize = 22;
                creator.Position = new Vector2(-42f, 12f);
                creator.Size = new Vector2(360f, 34f);
            }

            fadeAni = new Anime();
            fadeAni.AnimateFloat(alpha => title.Alpha = artist.Alpha = alpha)
                .AddTime(0f, 0f, EaseType.SineEaseOut)
                .AddTime(0.25f, 1f)
                .Build();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.SelectedMap.BindAndTrigger(OnMapChange);
            Model.PreferUnicode.OnNewValue += OnPreferUnicode;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.SelectedMap.OnNewValue -= OnMapChange;
            Model.PreferUnicode.OnNewValue -= OnPreferUnicode;
        }

        /// <summary>
        /// Displays label values based on current state.
        /// </summary>
        private void SetupLabels()
        {
            var map = Model.SelectedMap.Value;
            bool preferUnicode = Model.PreferUnicode.Value;

            if (map == null)
            {
                title.Text = artist.Text = creator.Text = "";
            }
            else
            {
                title.Text = map.Metadata.GetTitle(preferUnicode);
                artist.Text = map.Metadata.GetArtist(preferUnicode);
            }
            creator.Text = $"mapped by {map.Metadata.Creator}";
            fadeAni.PlayFromStart();
        }

        /// <summary>
        /// Event called on map selection change.
        /// </summary>
        private void OnMapChange(IPlayableMap map) => SetupLabels();

        /// <summary>
        /// Event called on unicode preference change.
        /// </summary>
        private void OnPreferUnicode(bool preferUnicode) => SetupLabels();
    }
}