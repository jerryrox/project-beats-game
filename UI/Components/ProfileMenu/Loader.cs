using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public class Loader : UguiObject, IHasAlpha {

        private const float RotationSpeed = -150f;

        private CanvasGroup canvasGroup;

        private LoaderIcon loader;
        private Blocker blocker;

        private IAnime showAni;
        private IAnime hideAni;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            blocker = CreateChild<Blocker>("blocker");
            {
                blocker.Anchor = AnchorType.Fill;
                blocker.Offset = Offset.Zero;
                blocker.Background.Color = new Color(0f, 0f, 0f, 0.5f);

                loader = blocker.CreateChild<LoaderIcon>("loader");
                {
                    loader.Size = new Vector2(72f, 72f);
                }
            }

            showAni = new Anime();
            showAni.AddEvent(0f, () => Active = true);
            showAni.AnimateFloat(alpha => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(alpha => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AddEvent(hideAni.Duration, () => Active = false);

            Active = false;
            Alpha = 0f;
        }

        /// <summary>
        /// Shows the loader display.
        /// </summary>
        public void Show()
        {
            hideAni.Stop();
            showAni.PlayFromStart();
        }

        /// <summary>
        /// Hides the loader display.
        /// </summary>
        public void Hide()
        {
            showAni.Stop();
            hideAni.PlayFromStart();
        }

        protected void Update()
        {
            loader.RotationZ += Time.deltaTime * RotationSpeed;
        }
    }
}