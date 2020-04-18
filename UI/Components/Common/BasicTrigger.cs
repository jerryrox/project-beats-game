using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// The basis of any triggers in PB.
    /// </summary>
    public class BasicTrigger : UguiTrigger {

        /// <summary>
        /// Event called on button trigger event.
        /// </summary>
        public event Action OnTriggered;

        /// <summary>
        /// Animation played on trigger.
        /// </summary>
        protected IAnime triggerAni;


        /// <summary>
        /// Returns whether the button fires "click" event on pointer click.
        /// If false, it is triggered on pointer down.
        /// </summary>
        protected virtual bool IsClickToTrigger => false;

        /// <summary>
        /// Returns the name of the sound to be played on pointer enter.
        /// </summary>
        protected virtual string PointerEnterAudio => "menuhit";

        /// <summary>
        /// Returns the name of the sound to be played on button trigger.
        /// </summary>
        protected virtual string TriggerAudio => "menuclick";

        [ReceivesDependency]
        protected ISoundPooler SoundPooler { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnPointerEnter += OnPointerEntered;
            OnPointerExit += OnPointerExited;
            if (IsClickToTrigger)
                OnPointerClick += OnClickTriggered;
            else
                OnPointerDown += OnClickTriggered;
        }

        /// <summary>
        /// Event called on pointer enter event.
        /// </summary>
        protected virtual void OnPointerEntered()
        {
            SoundPooler.Play(PointerEnterAudio);
        }

        /// <summary>
        /// Event called on pointer exit event.
        /// </summary>
        protected virtual void OnPointerExited() {}

        /// <summary>
        /// Event called on pointer down or click.
        /// </summary>
        protected virtual void OnClickTriggered()
        {
            if(triggerAni != null)
                triggerAni.PlayFromStart();

            SoundPooler.Play(TriggerAudio);
            OnTriggered?.Invoke();
        }
    }
}