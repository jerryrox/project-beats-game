using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download
{
    public class ResultLoader : UguiObject, IHasAlpha {

        private CanvasGroup canvasGroup;

        private ISprite bgSprite;
        private LoaderIcon loaderIcon;

        private IAnime showAni;
        private IAnime hideAni;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            bgSprite = CreateChild<UguiSprite>("bg", 0);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.Offset = Offset.Zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            loaderIcon = CreateChild<LoaderIcon>("loader", 1);
            {
                loaderIcon.Size = new Vector2(72f, 72f);
                loaderIcon.Rotate = true;
            }

            this.Alpha = 0f;

            showAni = new Anime();
            showAni.AddEvent(0f, () => Active = true);
            showAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AddEvent(hideAni.Duration, () => Active = false);

            Active = false;
        }

        /// <summary>
        /// Shows the loader.
        /// </summary>
        public void Show()
        {
            if(Active)
                return;

            hideAni.Stop();
            showAni.PlayFromStart();
        }

        /// <summary>
        /// Hides the loader.
        /// </summary>
        public void Hide()
        {
            if(!Active)
                return;

            showAni.Stop();
            hideAni.PlayFromStart();
        }
    }
}