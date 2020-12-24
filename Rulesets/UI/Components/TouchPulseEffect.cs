using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public class TouchPulseEffect : UguiSprite, IRecyclable<TouchPulseEffect> {

        private const float PulseStartSize = 140f;
        private const float PulseFullSize = 320f;

        private IAnime anime;


        public IRecycler<TouchPulseEffect> Recycler { get; set; }


        [InitWithDependency]
        private void Init()
        {
            SpriteName = "glow-in-square-32";
            ImageType = Image.Type.Sliced;
            RotationZ = 45f;

            AddEffect(new AdditiveShaderEffect());

            anime = new Anime();
            anime.AnimateFloat((alpha) => this.Alpha = alpha)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.1f, 1f, EaseType.QuadEaseIn)
                .AddTime(0.25f, 0f)
                .Build();
            anime.AnimateVector2((size) => this.Size = size)
                .AddTime(0f, new Vector2(PulseStartSize, PulseStartSize), EaseType.CubicEaseOut)
                .AddTime(0.25f, new Vector2(PulseFullSize, PulseFullSize))
                .Build();
            anime.AddEvent(anime.Duration, () => Recycler.Return(this));
        }

        /// <summary>
        /// Shows this effect at specified position.
        /// </summary>
        public void Show(Vector3 worldPos, Color tint)
        {
            myTransform.position = worldPos;
            this.Tint = tint;

            anime.PlayFromStart();
        }

        void IRecyclable.OnRecycleNew()
        {
            Active = true;
        }

        void IRecyclable.OnRecycleDestroy()
        {
            Active = false;
            anime.Stop();
        }
    }
}