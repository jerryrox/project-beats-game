using PBGame.UI.Models.Background;
using PBGame.Maps;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Background
{
    public abstract class BaseBackgroundDisplay : UguiObject, IBackgroundDisplay {

        protected IMapBackground background;

        private CanvasGroup canvasGroup;

        private IAnime enableAni;
        private IAnime disableAni;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public abstract BackgroundType Type { get; }

        public abstract Color Color { get; set; }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            enableAni = new Anime();
            enableAni.AnimateFloat((alpha) => Alpha = alpha)
                .AddTime(0f, () => Alpha)
                .AddTime(0.35f, 1f)
                .Build();

            disableAni = new Anime();
            disableAni.AnimateFloat((alpha) => Alpha = alpha)
                .AddTime(0f, () => Alpha)
                .AddTime(0.35f, 0f)
                .Build();
            disableAni.AddEvent(disableAni.Duration, () => Active = false);
        }

        public virtual void MountBackground(IMapBackground background) => this.background = background;

        public void ToggleDisplay(bool enable)
        {
            if (enable && !Active)
            {
                Active = true;
                disableAni.Stop();
                enableAni.PlayFromStart();
            }
            else if(!enable && Active)
            {
                enableAni.Stop();
                disableAni.PlayFromStart();
            }
        }
    }
}