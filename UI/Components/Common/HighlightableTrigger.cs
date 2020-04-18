using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Animations.Sections;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// Trigger with assumed highlighting bar sprite that reacts with focus state.
    /// </summary>
    public class HighlightableTrigger : FocusableTrigger
    {

        protected ISprite highlightSprite;

        protected IAnime highlightAni;
        protected IAnime unhighlightAni;


        /// <summary>
        /// Returns the stretched size of highlight sprite when the trigger is on focused state.
        /// </summary>
        protected virtual float HighlightedSize => IsHighlightSpriteVertical ? Height : Width;

        /// <summary>
        /// Returns the size of highlight sprite on the non-stretching side.
        /// </summary>
        protected virtual float NonHighlightedSize => 2;

        /// <summary>
        /// Returns the depth of the highlight sprite.
        /// </summary>
        protected virtual int HighlightSpriteDepth => 2;

        /// <summary>
        /// Anchoring of the highlight sprite.
        /// </summary>
        protected virtual Anchors HighlightSpriteAnchor => Anchors.BottomStretch;

        /// <summary>
        /// Returns the pivot of the highlight sprite based on the anchor.
        /// </summary>
        protected Pivots HighlightSpritePivot { get; private set; }

        /// <summary>
        /// Returns whether the highlight sprite should be stretched vertically.
        /// </summary>
        protected bool IsHighlightSpriteVertical { get; private set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            CacheHighlightProperties();

            highlightSprite = CreateChild<UguiSprite>("highlight", HighlightSpriteDepth);
            {
                highlightSprite.Anchor = HighlightSpriteAnchor;
                highlightSprite.Pivot = HighlightSpritePivot;
                if (IsHighlightSpriteVertical)
                    highlightSprite.Size = new Vector2(NonHighlightedSize, 0f);
                else
                    highlightSprite.Size = new Vector2(0f, NonHighlightedSize);
                highlightSprite.Position = Vector2.zero;
                highlightSprite.Color = colorPreset.PrimaryFocus;
                highlightSprite.Alpha = 0f;
            }
        }

        /// <summary>
        /// Creates default highlighting animation.
        /// </summary>
        protected void UseDefaultHighlightAni()
        {
            // Create highlight animation
            highlightAni = new Anime();
            highlightAni.AnimateFloat(a => highlightSprite.Alpha = a)
                .AddTime(0f, () => highlightSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();
            // Highlight sprite sizing animation.
            ISection<float> highlightSection = null;
            if (IsHighlightSpriteVertical)
                highlightSection = highlightAni.AnimateFloat(size => highlightSprite.Height = size).AddTime(0f, () => highlightSprite.Height);
            else
                highlightSection = highlightAni.AnimateFloat(size => highlightSprite.Width = size).AddTime(0f, () => highlightSprite.Width);
            highlightSection.AddTime(0.25f, () => HighlightedSize).Build();

            // Create unhighlight animation.
            unhighlightAni = new Anime();
            unhighlightAni.AnimateFloat(a => highlightSprite.Alpha = a)
                .AddTime(0f, () => highlightSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            // Highlight sprite sizing animation.
            ISection<float> unhighlightSection = null;
            if (IsHighlightSpriteVertical)
                unhighlightSection = unhighlightAni.AnimateFloat(size => highlightSprite.Height = size).AddTime(0f, () => highlightSprite.Height);
            else
                unhighlightSection = unhighlightAni.AnimateFloat(size => highlightSprite.Width = size).AddTime(0f, () => highlightSprite.Width);
            unhighlightSection.AddTime(0.25f, 0f).Build();
        }

        protected override void OnFocusAniStop()
        {
            base.OnFocusAniStop();
            highlightAni?.Stop();
            unhighlightAni?.Stop();
        }

        protected override void OnFocusAniPlay(bool animate)
        {
            base.OnFocusAniPlay(animate);
            if (highlightAni != null)
            {
                if(animate)
                    highlightAni.PlayFromStart();
                else
                    highlightAni.Seek(highlightAni.Duration);
            }
        }

        protected override void OnUnfocusAniPlay(bool animate)
        {
            base.OnUnfocusAniPlay(animate);
            if (unhighlightAni != null)
            {
                if (animate)
                    unhighlightAni.PlayFromStart();
                else
                    unhighlightAni.Seek(unhighlightAni.Duration);
            }
        }

        /// <summary>
        /// Caches some properties of the highlight sprite.
        /// </summary>
        private void CacheHighlightProperties()
        {
            // Determin pivot of the highlight sprite.
            switch (HighlightSpriteAnchor)
            {
                case Anchors.Top:
                case Anchors.TopStretch:
                    HighlightSpritePivot = Pivots.Top;
                    IsHighlightSpriteVertical = false;
                    break;

                case Anchors.Left:
                case Anchors.LeftStretch:
                    HighlightSpritePivot = Pivots.Left;
                    IsHighlightSpriteVertical = true;
                    break;

                case Anchors.Right:
                case Anchors.RightStretch:
                    HighlightSpritePivot = Pivots.Right;
                    IsHighlightSpriteVertical = true;
                    break;

                case Anchors.Bottom:
                case Anchors.BottomStretch:
                    HighlightSpritePivot = Pivots.Bottom;
                    IsHighlightSpriteVertical = false;
                    break;
            }
            throw new Exception("Unsupported highlight sprite anchor type: " + HighlightSpriteAnchor);
        }
    }
}