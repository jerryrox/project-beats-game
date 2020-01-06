using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations
{
    public abstract class NavigationView : UguiNavigationView {

        /// <summary>
        /// Animation to be played upon showing.
        /// </summary>
        private IAnime showAnimation;

        /// <summary>
        /// Animation to be played upon hiding.
        /// </summary>
        private IAnime hideAnimation;


        [InitWithDependency]
        private void Init()
        {
            Anchor = Anchors.Fill;
            RawSize = Vector2.zero;
        }

        protected override bool OnPreShowView() => PreProcessView(
            ref showAnimation,
            CreateShowAnimation,
            base.OnPreShowView,
            OnPostShowView
        );

        protected override bool OnPreHideView() => PreProcessView(
            ref hideAnimation,
            CreateHideAnimation,
            base.OnPreHideView,
            OnPostHideView
        );

        /// <summary>
        /// Creates a new instance of anime to be played on view show event.
        /// </summary>
        protected virtual IAnime CreateShowAnimation() => null;

        /// <summary>
        /// Creates a new instance of anime to be played on view hide event.
        /// </summary>
        protected virtual IAnime CreateHideAnimation() => null;

        /// <summary>
        /// Handles the shared logic of PreHide or PreShow view methods.
        /// </summary>
        private bool PreProcessView(ref IAnime ani, Func<IAnime> createAni, Func<bool> defaultAction, Action postAction)
        {
            // If show animation is null, try creating an instance.
            if (ani == null)
            {
                ani = createAni.Invoke();
                // If still null, there is no animation.
                if (ani == null)
                {
                    return defaultAction.Invoke();
                }

                // Else, hook an event on this animation at end.
                ani.AddEvent(ani.Duration, postAction);
            }
            // Play the animation.
            ani.PlayFromStart();
            return false;
        }
    }
}