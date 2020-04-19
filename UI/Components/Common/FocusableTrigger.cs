using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// Trigger with assumed focus/unfocusing effect 
    /// </summary>
    public class FocusableTrigger : HoverableTrigger
    {

        /// <summary>
        /// Event called on trigger state change.
        /// </summary>
        public event Action<bool> OnFocused;

        protected ISprite focusSprite;

        protected IAnime focusAni;
        protected IAnime unfocusAni;

        private bool isFocused = false;


        /// <summary>
        /// Whether the trigger is currently focused.
        /// </summary>
        public bool IsFocused
        {
            get => isFocused;
            set => SetFocused(value, true);
        }

        /// <summary>
        /// Returns the depth of the focus sprite.
        /// </summary>
        protected virtual int FocusSpriteDepth => 1;


        [InitWithDependency]
        private void Init()
        {
            focusSprite = CreateChild<UguiSprite>("focus", FocusSpriteDepth);
            {
                focusSprite.Anchor = Anchors.Fill;
                focusSprite.RawSize = Vector2.zero;
                focusSprite.Position = Vector2.zero;
                focusSprite.Alpha = 0f;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            focusAni?.Stop();
            unfocusAni?.Stop();
        }

        /// <summary>
        /// Creates default focus/unfocus animations.
        /// </summary>
        public void UseDefaultFocusAni()
        {
            focusAni = new Anime();
            focusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.25f, 0.25f)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Sets the focused stated of the trigger.
        /// </summary>
        protected void SetFocused(bool focused, bool animate)
        {
            if (this.isFocused == focused)
                return;

            this.isFocused = focused;
            OnFocusAniStop();

            if (focused)
                OnFocusAniPlay(animate);
            else
                OnUnfocusAniPlay(animate);

            OnFocused?.Invoke(focused);
        }

        /// <summary>
        /// Event called on focus and unfocus animation stop.
        /// </summary>
        protected virtual void OnFocusAniStop()
        {
            focusAni?.Stop();
            unfocusAni?.Stop();
        }

        /// <summary>
        /// Event called on focus animation play.
        /// </summary>
        protected virtual void OnFocusAniPlay(bool animate)
        {
            if (focusAni != null)
            {
                if (focusAni != null)
                {
                    if (animate)
                        focusAni.PlayFromStart();
                    else
                        focusAni.Seek(focusAni.Duration);
                }
            }
        }

        /// <summary>
        /// Event called on unfocus animation play.
        /// </summary>
        protected virtual void OnUnfocusAniPlay(bool animate)
        {
            if (unfocusAni != null)
            {
                if (animate)
                    unfocusAni.PlayFromStart();
                else
                    unfocusAni.Seek(unfocusAni.Duration);
            }
        }
    }
}