using PBGame.UI.Components.Common;
using PBGame.Maps;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class SortButton : HighlightableTrigger, IHasLabel {

        private ILabel label;


        /// <summary>
        /// The sort type represented by this button.
        /// </summary>
        public MapsetSorts SortType { get; set; }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            label = CreateChild<Label>("label", 10);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.FontSize = 18;
            }

            highlightSprite.Color = colorPreset.SecondaryFocus;
            highlightSprite.Alpha = 0f;

            UseDefaultFocusAni();
            UseDefaultHighlightAni();
            UseDefaultHoverAni();
        }
    }
}