using System;
using System.Collections;
using System.Collections.Generic;
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

        private ISprite dark;
        private ISprite loader;
        private IRaycastable pointerBlocker;

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

            dark = CreateChild<UguiSprite>("dark", 0);
            {
                dark.Anchor = Anchors.Fill;
                dark.RawSize = Vector2.zero;
                dark.Color = new Color(0f, 0f, 0f, 0.5f);

                pointerBlocker = dark as IRaycastable;

                loader = dark.CreateChild<UguiSprite>("loader", 1);
                {
                    loader.Size = new Vector2(72f, 72f);
                    loader.SpriteName = "loader";
                    loader.Color = colorPreset.PrimaryFocus;

                    if(loader is IRaycastable raycastable)
                        raycastable.IsRaycastTarget = false;
                }
            }

            if(pointerBlocker != null)
                pointerBlocker.IsRaycastTarget = false;

            Alpha = 0f;

            showAni = new Anime();
            showAni.AnimateFloat(alpha => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(alpha => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            if (pointerBlocker != null)
                hideAni.AddEvent(0.25f, () => pointerBlocker.IsRaycastTarget = false);
        }

        /// <summary>
        /// Shows the loader display.
        /// </summary>
        public void Show()
        {
            if(pointerBlocker != null)
                pointerBlocker.IsRaycastTarget = true;

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

        private void Update()
        {
            if(pointerBlocker == null || pointerBlocker.IsRaycastTarget)
                loader.RotationZ += Time.deltaTime * RotationSpeed;
        }
    }
}