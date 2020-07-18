using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class AdvancedButton : BoxButton {

        protected override float HoveredInAlpha => 0.5f;

        protected override float HoveredOutAlpha => 0.25f;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            LabelText = "Advanced";
            
            hoverSprite.SpriteName = "circle-16";
            hoverSprite.ImageType = Image.Type.Sliced;
        }
    }
}