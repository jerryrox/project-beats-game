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
    public class BasicTrigger : UguiTrigger, IHasIcon {

        /// <summary>
        /// Event called on button trigger event.
        /// </summary>
        public event Action OnTriggered;

        /// <summary>
        /// Animation played on trigger.
        /// </summary>
        protected IAnime triggerAni;

        /// <summary>
        /// Icon sprite on the trigger, if created.
        /// </summary>
        protected ISprite iconSprite;


        public virtual string IconName
        {
            get => iconSprite == null ? null : iconSprite.SpriteName;
            set
            {
                if(iconSprite != null)
                    iconSprite.SpriteName = value;
            }
        }

        /// <summary>
        /// Whether the button fires "click" event on pointer click.
        /// If false, it is triggered on pointer down.
        /// </summary>
        public bool IsClickToTrigger { get; set; } = false;

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
            OnPointerClick += OnPointerClicked;
            OnPointerDown += OnPointerDowned;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            triggerAni?.Stop();
        }

        /// <summary>
        /// Creates a new icon sprite for the trigger and returns it.
        /// </summary>
        public ISprite CreateIconSprite(int depth = 5, string spriteName = null, float size = 36f, float alpha = 0.65f)
        {
            if(iconSprite != null)
                return iconSprite;

            iconSprite = CreateChild<UguiSprite>("icon", depth);
            iconSprite.Position = Vector2.zero;
            if(!string.IsNullOrEmpty(spriteName))
                iconSprite.SpriteName = spriteName;
            iconSprite.Size = new Vector2(size, size);
            iconSprite.Alpha = alpha;
            return iconSprite;
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
        /// Event called on pointer click event.
        /// </summary>
        protected virtual void OnPointerClicked()
        {
            if(IsClickToTrigger)
                OnClickTriggered();
        }

        /// <summary>
        /// Event called on pointer down event.
        /// </summary>
        protected virtual void OnPointerDowned()
        {
            if(!IsClickToTrigger)
                OnClickTriggered();
        }

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