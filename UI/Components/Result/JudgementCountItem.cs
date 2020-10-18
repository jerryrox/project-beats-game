using PBGame.Graphics;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Result
{
    public class JudgementCountItem : UguiObject {

        private ISprite baseSprite;
        private ISprite bgSprite;
        private Label countLabel;


        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            baseSprite = CreateChild<UguiSprite>("base");
            {
                baseSprite.Anchor = AnchorType.BottomStretch;
                baseSprite.Offset = new Offset(-14f, 0f);
                baseSprite.Y = 0f;
                baseSprite.Height = 24f;
                baseSprite.SpriteName = "glow-bar";
                baseSprite.ImageType = Image.Type.Sliced;
            }
            bgSprite = CreateChild<UguiSprite>("bg");
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = Offset.Zero;
                bgSprite.SpriteName = "gradation-bottom";
                bgSprite.ImageType = Image.Type.Sliced;
            }
            countLabel = CreateChild<Label>("count");
            {
                countLabel.Anchor = AnchorType.Fill;
                countLabel.Offset = Offset.Zero;
                countLabel.Position = new Vector3(0f, -4f);
                countLabel.FontSize = 20;
            }
        }

        /// <summary>
        /// Sets the type of hit result this item should represent.
        /// </summary>
        public void SetResultType(HitResultType type)
        {
            var color = ColorPreset.GetHitResultColor(type);
            baseSprite.Color = color;
            bgSprite.Color = color.Alpha(0.25f);
        }

        /// <summary>
        /// Sets the number of hits achieved for this item.
        /// </summary>
        public void SetCount(int count)
        {
            countLabel.Text = count.ToString("N0");
        }
    }
}