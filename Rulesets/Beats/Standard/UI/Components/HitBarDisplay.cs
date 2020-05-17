using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class HitBarDisplay : UguiSprite {

        private IGraphicObject effectHolder;
        private ISprite hitBarSprite;
        private ISprite followSprite;

        private IAnime holdAni;
        private IAnime releaseAni;

        private BeatsCursor cursor;

        private ManagedRecycler<JudgementEffect> effectRecycler;


        [InitWithDependency]
        private void Init()
        {
            this.Alpha = 0f;

            effectRecycler = new ManagedRecycler<JudgementEffect>(CreateEffect);

            effectHolder = CreateChild("effect-holder", 0);
            {
                effectHolder.Size = Vector2.zero;

                effectRecycler.Precook(6);
            }
            hitBarSprite = CreateChild<UguiSprite>("bar", 1);
            {
                hitBarSprite.Anchor = AnchorType.MiddleStretch;
                hitBarSprite.SetOffsetHorizontal(0f);
                hitBarSprite.SpriteName = "glow-bar";
                hitBarSprite.Y = 0f;
                hitBarSprite.ImageType = Image.Type.Sliced;
                hitBarSprite.Alpha = 0.5f;
            }
            followSprite = CreateChild<UguiSprite>("follow", 2);
            {
                followSprite.SpriteName = "glow-128";
                followSprite.Size = new Vector2(500f, 500f);
                followSprite.Alpha = 0f;
            }

            holdAni = new Anime();
            holdAni.AnimateFloat(a => hitBarSprite.Alpha = a)
                .AddTime(0f, () => hitBarSprite.Alpha)
                .AddTime(0.1f, 1f)
                .Build();
            holdAni.AnimateVector3(s => followSprite.Scale = s)
                .AddTime(0f, () => followSprite.Scale)
                .AddTime(0.1f, Vector3.one)
                .Build();
            holdAni.AnimateFloat(a => followSprite.Alpha = a)
                .AddTime(0f, () => followSprite.Alpha)
                .AddTime(0.1f, 1f)
                .Build();

            releaseAni = new Anime();
            releaseAni.AnimateFloat(a => hitBarSprite.Alpha = a)
                .AddTime(0f, () => hitBarSprite.Alpha)
                .AddTime(0.1f, 0.5f)
                .Build();
            releaseAni.AnimateVector3(s => followSprite.Scale = s)
                .AddTime(0f, () => followSprite.Scale)
                .AddTime(0.1f, new Vector3(2f, 2f, 1f))
                .Build();
            releaseAni.AnimateFloat(a => followSprite.Alpha = a)
                .AddTime(0f, () => followSprite.Alpha)
                .AddTime(0.1f, 0f)
                .Build();
        }

        /// <summary>
        /// Associates this display with the specified cursor.
        /// </summary>
        public void LinkCursor(BeatsCursor cursor)
        {
            this.UnlinkCursor();
            if(cursor == null)
                return;

            this.cursor = cursor;
            cursor.IsOnHitBar.BindAndTrigger(OnHitBarHover);
        }

        /// <summary>
        /// Removes association with currently linked cursor.
        /// </summary>
        public void UnlinkCursor()
        {
            if(cursor == null)
                return;

            cursor.IsOnHitBar.OnValueChanged -= OnHitBarHover;
            SetHold(false);
        }

        /// <summary>
        /// Sets hit bar sprite's visual state for specified flag.
        /// </summary>
        public void SetHold(bool isHolding)
        {
            if (isHolding)
            {
                releaseAni.Stop();
                holdAni.PlayFromStart();
            }
            else
            {
                holdAni.Stop();
                releaseAni.PlayFromStart();
            }
        }

        /// <summary>
        /// Shows a new judgement effect for specified hit object.
        /// </summary>
        public void ShowJudgementEffect(float x, HitObjectView hitObjectView)
        {
            var effect = effectRecycler.GetNext();
            effect.X = x;
            effect.ShowEffect(hitObjectView);
        }

        protected void Update()
        {
            if(cursor == null)
                return;
            if(!cursor.IsOnHitBar.Value)
                return;

            followSprite.X = cursor.HitBarPos;
        }

        /// <summary>
        /// Creates a new judgement effect object.
        /// </summary>
        private JudgementEffect CreateEffect() => effectHolder.CreateChild<JudgementEffect>(depth: effectRecycler.TotalCount);

        /// <summary>
        /// Event from cursor when hit bar hover state has changed.
        /// </summary>
        private void OnHitBarHover(bool isOnHitBar, bool wasOnHitBar) => SetHold(isOnHitBar);
    }
}