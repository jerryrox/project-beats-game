using PBGame.UI.Components.Common;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.HomeMenu
{
    public class MenuButton : HoverableTrigger, IHasLabel {

        private ISprite pulseSprite;
        private ISprite flashSprite;
        private ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public override string IconName
        {
            get => base.IconName;
            set
            {
                base.IconName = value;
                pulseSprite.SpriteName = value;
            }
        }


        [InitWithDependency]
        private void Init(ISoundPool soundPooler)
        {
            CreateIconSprite(depth: 2, size: 64f);

            pulseSprite = CreateChild<UguiSprite>("pulse-icon", 3);
            {
                pulseSprite.Size = new Vector2(64f, 64f);
                pulseSprite.Alpha = 0f;

                pulseSprite.IsRaycastTarget = false;
            }
            flashSprite = CreateChild<UguiSprite>("flash", 0);
            {
                flashSprite.Anchor = AnchorType.Fill;
                flashSprite.RawSize = Vector2.zero;
                flashSprite.SpriteName = "glow-128";
                flashSprite.Alpha = 0f;
            }
            label = CreateChild<Label>("label", 1);
            {
                label.Y = -52f;
                label.IsBold = true;
                label.FontSize = 24;
            }

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat((alpha) => flashSprite.Alpha = alpha)
                .AddTime(0f, () => flashSprite.Alpha)
                .AddTime(0.5f, 0.5f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat((alpha) => flashSprite.Alpha = alpha)
                .AddTime(0f, () => flashSprite.Alpha)
                .AddTime(0.5f, 0f)
                .Build();

            triggerAni = new Anime();
            triggerAni.AnimateFloat((alpha) => pulseSprite.Alpha = alpha)
                .AddTime(0f, 0.5f, EaseType.QuartEaseIn)
                .AddTime(1f, 0f)
                .Build();
            triggerAni.AnimateVector3((scale) => pulseSprite.Scale = scale)
                .AddTime(0f, Vector3.one, EaseType.QuartEaseOut)
                .AddTime(1f, new Vector3(5f, 5f, 1f))
                .Build();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            pulseSprite.Alpha = 0f;
            pulseSprite.Scale = Vector3.one;
        }

        protected override void OnClickTriggered()
        {
            base.OnClickTriggered();
            InvokeExit();
        }
    }
}