using System;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI
{
    public abstract class GameplayLayer : UguiObject, IHasAlpha {

        private CanvasGroup canvasGroup;


        /// <summary>
        /// Returns the animation for showing the gameplay gui.
        /// </summary>
        public IAnime ShowAni { get; private set; }

        /// <summary>
        /// Returns the animation for hiding the gameplay gui.
        /// </summary>
        public IAnime HideAni { get; private set; }

        /// <summary>
        /// Returns the play area container object.
        /// </summary>
        public PlayAreaContainer PlayArea { get; private set; }

        /// <summary>
        /// Returns the hud container object.
        /// </summary>
        public HudContainer Hud { get; private set; }

        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            PlayArea = CreatePlayArea();
            {
                PlayArea.Depth = 0;
            }
            Hud = CreateHud();
            {
                Hud.Depth = 1;
            }

            ShowAni = new Anime();
            ShowAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            HideAni = new Anime();
            HideAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Creates a new play area container.
        /// </summary>
        protected abstract PlayAreaContainer CreatePlayArea();

        /// <summary>
        /// Creates a new hud container.
        /// </summary>
        protected abstract HudContainer CreateHud();
    }
}