using PBGame.Maps;
using PBGame.Audio;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Songs
{
    public class SortButton : HighlightTrigger, ISortButton {

        public MapsetSorts SortType { get; set; }

        protected override float HighlightWidth => 120f;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            highlightSprite.Color = colorPreset.SecondaryFocus;
            label.IsBold = false;
        }
    }
}