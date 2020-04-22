using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.GameLoad
{
    public class LoadIndicator : UguiObject, IGameLoadComponent {

        private const float RotationSpeed = 110f;

        private CanvasGroup canvasGroup;
        
        private ISprite loadSprite;

        private IAnime showAni;
        private IAnime hideAni;


        public float ShowAniDuration => showAni.Duration;

        public float HideAniDuration => hideAni.Duration;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            loadSprite = CreateChild<UguiSprite>("load", 0);
            {
                loadSprite.Anchor = Anchors.Fill;
                loadSprite.Offset = Offset.Zero;
                loadSprite.SpriteName = "loader";
                loadSprite.Color = colorPreset.PrimaryFocus;
            }

            showAni = new Anime();
            showAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(1.5f, 0f, EaseType.QuadEaseOut)
                .AddTime(1.75f, 1f)
                .Build();
            showAni.AnimateFloat(scale => Scale = new Vector3(scale, scale, 1f))
                .AddTime(0f, 1f)
                .AddTime(1.5f, 2f, EaseType.QuadEaseOut)
                .AddTime(1.75f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AddEvent(0f, () => showAni.Stop());
            hideAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(0.5f, 0f)
                .Build();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            showAni.Stop();
            hideAni.Stop();
        }

        public void Show() => showAni.PlayFromStart();

        public void Hide() => hideAni.PlayFromStart();

        private void Update()
        {
            RotationZ -= Time.deltaTime * RotationSpeed;
        }
    }
}