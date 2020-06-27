using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public abstract class BaseLoginView : UguiObject, ILoginView {

        private CanvasGroup canvasGroup;

        private ISprite bgSprite;

        private IAnime showAni;
        private IAnime hideAni;

        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        /// <summary>
        /// The API instance assigned to interact with.
        /// </summary>
        public IApi Api { get; private set; }

        public abstract float DesiredHeight { get; }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();

            bgSprite = CreateChild<UguiSprite>("bg");
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = Offset.Zero;
                bgSprite.Color = new Color(1f, 1f, 1f, 0.0625f);
            }

            showAni = new Anime();
            showAni.AddEvent(0f, () =>
            {
                hideAni.Stop();
                Active = true;
            });
            showAni.AnimateFloat(a => Alpha = a)
                .AddTime(0f, () => Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AddEvent(0f, showAni.Stop);
            hideAni.AnimateFloat(a => Alpha = a)
                .AddTime(0f, () => Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AddEvent(hideAni.Duration, () => Active = true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (showAni != null)
            {
                showAni.Stop();
                hideAni.Stop();
            }
        }

        public virtual void Setup(IApi api)
        {
            this.Api = api;
        }

        public void Show()
        {
            showAni.PlayFromStart();
        }

        public void Hide()
        {
            hideAni.PlayFromStart();
        }
    }
}