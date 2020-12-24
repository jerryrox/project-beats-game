using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// The basis of any triggers in PB.
    /// Supports hold but note that it won't work if trigger is set to work on pointer down instead of click.
    /// </summary>
    public class BasicTrigger : UguiTrigger, IHasIcon {

        /// <summary>
        /// The duration of press & hold required to trigger hold event.
        /// </summary>
        private const float HoldThreshold = 0.75f;

        /// <summary>
        /// Max distance from pointer down position before a hold is cancelled for dragging.
        /// </summary>
        private const float DragThreshold = 20f;

        /// <summary>
        /// Event called on button trigger event.
        /// </summary>
        public event Action OnTriggered;

        /// <summary>
        /// Event called on hold action event.
        /// </summary>
        public event Action OnHold;

        /// <summary>
        /// Animation played on trigger.
        /// </summary>
        protected IAnime triggerAni;

        /// <summary>
        /// Icon sprite on the trigger, if created.
        /// </summary>
        protected ISprite iconSprite;

        /// <summary>
        /// Amount of time left until triggering hold event.
        /// </summary>
        private float holdTime;

        /// <summary>
        /// Cursor position recorded on hold start.
        /// </summary>
        private Vector2 holdPos;

        /// <summary>
        /// A flag used to check during click trigger whether the pointer was already consumed through Hold event.
        /// </summary>
        private bool didHold = false;

        /// <summary>
        /// Whether button hover sound should be played.
        /// Updated from game configuration.
        /// </summary>
        private bool useButtonHoverSound;


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
        /// Default: true
        /// </summary>
        public bool IsClickToTrigger { get; set; } = true;

        /// <summary>
        /// Returns the name of the sound to be played on pointer enter.
        /// </summary>
        protected virtual string PointerEnterAudio => "menuhit";

        /// <summary>
        /// Returns the name of the sound to be played on button trigger.
        /// </summary>
        protected virtual string TriggerAudio => "menuclick";

        [ReceivesDependency]
        protected ISoundPool SoundPool { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnPointerEnter += OnPointerEntered;
            OnPointerExit += OnPointerExited;
            OnPointerClick += OnPointerClicked;
            OnPointerDown += OnPointerDowned;
            OnPointerUp += OnPointerUpped;

            if (GameConfiguration != null)
                GameConfiguration.UseButtonHoverSound.BindAndTrigger(OnUseButtonHoverSoundChange);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StopHold();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            triggerAni?.Stop();
        }

        protected virtual void OnDestroy()
        {
            GameConfiguration.UseButtonHoverSound.OnNewValue -= OnUseButtonHoverSoundChange;
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
            if(useButtonHoverSound && !string.IsNullOrEmpty(PointerEnterAudio))
                SoundPool.Play(PointerEnterAudio);
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
            if (IsClickToTrigger)
            {
                if(didHold)
                    return;
                OnClickTriggered();
            }
        }

        /// <summary>
        /// Event called on pointer down event.
        /// </summary>
        protected virtual void OnPointerDowned()
        {
            StopHold();

            if(!IsClickToTrigger)
                OnClickTriggered();
            else
                StartHold();
        }

        /// <summary>
        /// Event called on pointer up event.
        /// </summary>
        protected virtual void OnPointerUpped()
        {
            if(didHold)
                return;
            StopHold();
        }

        /// <summary>
        /// Event called on pointer down or click.
        /// </summary>
        protected virtual void OnClickTriggered()
        {
            if(triggerAni != null)
                triggerAni.PlayFromStart();

            StopHold();

            if(!string.IsNullOrEmpty(TriggerAudio))
                SoundPool.Play(TriggerAudio);
            OnTriggered?.Invoke();
        }

        protected virtual void Update()
        {
            if (holdTime > 0f)
            {
                holdTime -= Time.deltaTime;
                if (holdTime <= 0f)
                {
                    // Trigger hold only if not moved away from hold pos.
                    Vector2 mousePos = Input.mousePosition;
                    if (Mathf.Abs(mousePos.x - holdPos.x) < DragThreshold && Mathf.Abs(mousePos.y - holdPos.y) < DragThreshold)
                    {
                        // Invoke hold action.
                        didHold = true;
                        OnHold?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// Starts detection of hold action.
        /// </summary>
        private void StartHold()
        {
            holdTime = HoldThreshold;
            holdPos = Input.mousePosition;
        }

        /// <summary>
        /// Forcibly stops detection of hold action for current pointer.
        /// </summary>
        private void StopHold()
        {
            holdTime = -1f;
            didHold = false;
        }

        /// <summary>
        /// Event called from game configuration when button hover sound option is toggled.
        /// </summary>
        private void OnUseButtonHoverSoundChange(bool use)
        {
            useButtonHoverSound = use;
        }
    }
}