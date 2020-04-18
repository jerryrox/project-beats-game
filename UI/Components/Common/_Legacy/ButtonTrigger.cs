using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components
{
    public abstract class ButtonTrigger : UguiTrigger, IButtonTrigger {

        public event Action OnTriggered;

        protected IAnime hoverAni;
        protected IAnime outAni;
        protected IAnime triggerAni;


        /// <summary>
        /// Returns whether the button fires "click" event on pointer click.
        /// If false, it is triggered on pointer down.
        /// </summary>
        protected virtual bool IsClickToTrigger => true;

        [ReceivesDependency]
        protected ISoundPooler SoundPooler { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnPointerEnter += OnPointerEntered;
            OnPointerExit += OnPointerExited;
            if(IsClickToTrigger)
                OnPointerClick += OnClickTriggered;
            else
                OnPointerDown += OnClickTriggered;
        }

        /// <summary>
        /// Event called on pointer enter event.
        /// </summary>
        protected virtual void OnPointerEntered()
        {
            SoundPooler.Play("menuhit");

            outAni?.Stop();
            hoverAni?.PlayFromStart();
        }

        /// <summary>
        /// Event called on pointer exit event.
        /// </summary>
        protected virtual void OnPointerExited()
        {
            hoverAni?.Stop();
            outAni?.PlayFromStart();
        }

        /// <summary>
        /// Event called on pointer down or click.
        /// </summary>
        protected virtual void OnClickTriggered()
        {
            SoundPooler.Play("menuclick");
            triggerAni?.PlayFromStart();

            OnTriggered?.Invoke();
        }
    }
}