using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Maps;
using PBGame.Data.Records;
using PBGame.Rulesets.Scoring;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

using ShadowEffect = PBFramework.Graphics.Effects.CoffeeUI.ShadowEffect;

namespace PBGame.UI.Components.Result
{
    public class RankCircle : UguiObject {

        private UguiSprite meterSprite;
        private RankCircleRange rangeDisplay;
        private GradientEffect meterGradient;
        private MapImageDisplay mapThumb;
        private UguiSprite rankGlowSprite;
        private Label rankLabel;
        private ShadowEffect rankShadowEffect;
        private Label accuracyLabel;


        [ReceivesDependency]
        private ResultModel Model { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var bg = CreateChild<UguiSprite>("bg");
            {
                bg.Anchor = AnchorType.Fill;
                bg.SpriteName = "circle-320";
                bg.Color = ColorPreset.DarkBackground;
                bg.Offset = Offset.Zero;
            }
            meterSprite = CreateChild<UguiSprite>("meter");
            {
                meterSprite.Anchor = AnchorType.Fill;
                meterSprite.Offset = Offset.Zero;
                meterSprite.SpriteName = "circle-320";

                meterGradient = meterSprite.AddEffect(new GradientEffect());
                meterGradient.Component.direction = UIGradient.Direction.Vertical;
                meterGradient.Component.color1 = Color.white;
                meterGradient.Component.color2 = HexColor.Create("A0A0A0");
                meterGradient.Component.offset = 0.75f;
            }
            rangeDisplay = CreateChild<RankCircleRange>("ranges");
            {
                rangeDisplay.Anchor = AnchorType.Fill;
                rangeDisplay.Offset = new Offset(18f);
            }
            var mask = CreateChild<UguiSprite>("mask");
            {
                mask.Anchor = AnchorType.Fill;
                mask.Offset = new Offset(28f);
                mask.Color = ColorPreset.DarkBackground;
                mask.SpriteName = "circle-320";
                mask.AddEffect(new MaskEffect());

                mapThumb = mask.CreateChild<MapImageDisplay>("thumb");
                {
                    mapThumb.Anchor = AnchorType.Fill;
                    mapThumb.Offset = Offset.Zero;
                    mapThumb.Color = new Color(1f, 1f, 1f, 0.25f);
                }
            }
            rankGlowSprite = CreateChild<UguiSprite>("rank-glow");
            {
                rankGlowSprite.Size = new Vector2(256f, 256f);
                rankGlowSprite.SpriteName = "glow-128";
                rankGlowSprite.Alpha = 0.625f;
            }
            rankLabel = CreateChild<Label>("rank");
            {
                rankLabel.IsBold = true;
                rankLabel.FontSize = 128;

                rankShadowEffect = rankLabel.AddEffect(new ShadowEffect());
                rankShadowEffect.Component.style = ShadowStyle.Outline8;
            }
            var accBg = CreateChild<UguiSprite>("acc-bg");
            {
                accBg.Position = new Vector3(0f, -84f);
                accBg.Size = new Vector2(280f, 64f);
                accBg.SpriteName = "glow-128";
                accBg.Color = Color.black;
            }
            accuracyLabel = CreateChild<Label>("accuracy");
            {
                accuracyLabel.Position = accBg.Position;
                accuracyLabel.FontSize = 22;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Record.BindAndTrigger(OnRecordChanged);
            Model.MapBackground.BindAndTrigger(OnMapBgChanged);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Record.Unbind(OnRecordChanged);
            Model.MapBackground.Unbind(OnMapBgChanged);
        }

        /// <summary>
        /// Event called on record instance change.
        /// </summary>
        private void OnRecordChanged(IRecord record)
        {
            RankType rank = record?.Rank ?? RankType.D;
            float accuracy = record?.Accuracy ?? 0f;

            ColorPalette rankColor = ColorPreset.GetRankColor(rank);
            Color rankOutlineColor = ColorPreset.GetRankOutlineColor(rank);

            meterSprite.FillAmount = accuracy;
            meterSprite.Color = rankColor;

            rangeDisplay.Active = record != null;

            rankGlowSprite.Tint = rankColor;

            rankLabel.Color = rankColor;
            rankLabel.Text = rank.ToString();
            rankShadowEffect.Component.effectColor = rankOutlineColor;

            accuracyLabel.Text = accuracy.ToString("P2");
        }

        /// <summary>
        /// Event called on map instance change.
        /// </summary>
        private void OnMapBgChanged(IMapBackground background)
        {
            mapThumb.SetBackground(background);
        }
    }
}