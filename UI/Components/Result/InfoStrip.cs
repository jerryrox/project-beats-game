using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Data.Records;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Result
{
    public class InfoStrip : UguiObject {

        private Label scoreLabel;
        private Label comboLabel;
        private AvatarDisplay avatarDisplay;
        private Label nameLabel;
        private Label dateLabel;

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private ResultModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var bg = CreateChild<UguiSprite>("bg");
            {
                bg.Anchor = AnchorType.Fill;
                bg.Offset = Offset.Zero;
                bg.Color = ColorPreset.Passive.Darken(0.5f);
            }
            scoreLabel = CreateChild<Label>("score");
            {
                scoreLabel.Pivot = PivotType.Left;
                scoreLabel.X = 200f;
                scoreLabel.Alignment = TextAnchor.MiddleLeft;
                scoreLabel.FontSize = 36;
            }
            comboLabel = CreateChild<Label>("combo");
            {
                comboLabel.Anchor = AnchorType.Right;
                comboLabel.Pivot = PivotType.Right;
                comboLabel.X = -16f;
                comboLabel.Alignment = TextAnchor.MiddleRight;
                comboLabel.FontSize = 28;
            }
            avatarDisplay = CreateChild<AvatarDisplay>("avatar");
            {
                avatarDisplay.Pivot = PivotType.Right;
                avatarDisplay.X = -200f;
                avatarDisplay.Size = new Vector2(64f, 64f);
                avatarDisplay.Color = Color.black;
            }
            nameLabel = CreateChild<Label>("name");
            {
                nameLabel.Pivot = PivotType.BottomRight;
                nameLabel.X = -288f;
                nameLabel.IsBold = true;
                nameLabel.FontSize = 24;
                nameLabel.Alignment = TextAnchor.LowerRight;
            }
            dateLabel = CreateChild<Label>("date");
            {
                dateLabel.Pivot = PivotType.TopRight;
                dateLabel.X = -288f;
                dateLabel.FontSize = 20;
                dateLabel.Alignment = TextAnchor.UpperRight;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Record.BindAndTrigger(OnRecordChanged);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Record.Unbind(OnRecordChanged);

            avatarDisplay.RemoveSource();
        }

        /// <summary>
        /// Event called on record instance change.
        /// </summary>
        private void OnRecordChanged(IRecord record)
        {
            scoreLabel.Text = (record?.Score ?? 0).ToString("N0");
            comboLabel.Text = $"x{(record?.MaxCombo ?? 0).ToString("N0")}";
            nameLabel.Text = record?.Username ?? "";
            dateLabel.Text = record?.Date.ToString("yyyy-MM-dd HH:mm:ss") ?? "";

            avatarDisplay.SetSource(record?.AvatarUrl);
        }
    }
}