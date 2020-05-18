using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class JudgementEffect : UguiSprite, IRecyclable<JudgementEffect> {

        private Anime effectAni;

        private float targetWidth = 0;


        public IRecycler<JudgementEffect> Recycler { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            SpriteName = "glow-128";
            IsRaycastTarget = false;
            Alpha = 0f;
            AddEffect(new AdditiveShaderEffect());

            effectAni = new Anime() { StopMode = StopModeType.None };
            effectAni.AnimateVector2(s => this.Size = s)
                .AddTime(0f, Vector2.zero, EaseType.CubicEaseOut)
                .AddTime(0.25f, () => new Vector2(targetWidth, 8000))
                .Build();
            effectAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, 1f, EaseType.QuadEaseIn)
                .AddTime(0.25f, 0f)
                .Build();
            effectAni.AddEvent(effectAni.Duration, () => Recycler.Return(this));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(effectAni != null)
                effectAni.Stop();
        }

        /// <summary>
        /// Starts showing the judgement effect.
        /// </summary>
        public void ShowEffect(HitObjectView hitObjectView)
        {
            targetWidth = hitObjectView.Width * 1.5f;
            Tint = ColorPreset.GetHitResultColor(hitObjectView.Result.HitResult).Base;
            effectAni.PlayFromStart();
        }

        void IRecyclable.OnRecycleNew()
        {
            Active = true;
        }

        void IRecyclable.OnRecycleDestroy()
        {
            Active = false;
            Alpha = 0f;
        }
    }
}