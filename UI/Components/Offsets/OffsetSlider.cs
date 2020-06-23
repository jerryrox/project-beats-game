using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Audio;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Offsets
{
    public class OffsetSlider : UguiObject, IHasLabel {

        private ILabel titleLabel;
        private ILabel offsetLabel;
        private BasicSlider offsetSlider;

        private IMusicOffset offset;


        public string LabelText
        {
            get => titleLabel.Text;
            set => titleLabel.Text = value;
        }

        /// <summary>
        /// Returns the current offset source if exists.
        /// </summary>
        public IMusicOffset CurOffset => offset;


        [InitWithDependency]
        private void Init()
        {
            titleLabel = CreateChild<Label>("title", 0);
            {
                titleLabel.Anchor = AnchorType.TopStretch;
                titleLabel.SetOffsetHorizontal(0f);
                titleLabel.Height = 32f;
                titleLabel.Y = -16f;
                titleLabel.Alignment = TextAnchor.MiddleCenter;
                titleLabel.IsBold = true;
                titleLabel.FontSize = 20;
            }
            offsetLabel = CreateChild<Label>("offset", 1);
            {
                offsetLabel.Anchor = AnchorType.TopStretch;
                offsetLabel.SetOffsetHorizontal(0f);
                offsetLabel.Height = 32f;
                offsetLabel.Y = -40f;
                offsetLabel.Alignment = TextAnchor.MiddleCenter;
                offsetLabel.FontSize = 18;
            }
            offsetSlider = CreateChild<BasicSlider>("slider", 2);
            {
                offsetSlider.Anchor = AnchorType.BottomStretch;
                offsetSlider.SetOffsetHorizontal(0f);
                offsetSlider.Y = 32f;
                offsetSlider.Height = 16f;
                offsetSlider.IsWholeNumber = true;
                offsetSlider.MinValue = -100f;
                offsetSlider.MaxValue = 100f;

                offsetSlider.OnChange += OnOffsetChange;
            }
        }

        /// <summary>
        /// Sets the source reference to have the offset value modified for.
        /// </summary>
        public void SetSource(IMusicOffset offset)
        {
            this.offset = null;
            if(offset != null)
                offsetSlider.Value = offset.Offset.Value;
            this.offset = offset;
        }

        /// <summary>
        /// Event called on offset change from slider.
        /// </summary>
        private void OnOffsetChange(float value)
        {
            offsetLabel.Text = (value >= 0 ? $"+{value}" : value.ToString());
            if(offset != null)
                offset.Offset.Value = (int)value;
        }
    }
}