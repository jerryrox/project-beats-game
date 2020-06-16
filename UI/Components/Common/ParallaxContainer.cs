using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class ParallaxContainer : UguiObject {

        private UguiSprite contentContainer;
        private Mask maskEffect;

        private bool autoUpdateBounds = true;
        private bool useMask = false;
        private bool useParallax = true;
        private float parallaxSpeed = 10f;
        private float parallaxScale = 1.05f;

        private Vector3 moveRange = new Vector3();
        private Vector3 curPos = Vector3.zero;

        private IAccelerator accelerator;


        /// <summary>
        /// Returns the content container.
        /// </summary>
        public UguiSprite Content => contentContainer;

        /// <summary>
        /// Whether the content should continuously adapt to this container's size in Update() calls.
        /// This will have performance implications.
        /// Default: true
        /// </summary>
        public bool AutoUpdateBounds
        {
            get => autoUpdateBounds;
            set => autoUpdateBounds = value;
        }

        /// <summary>
        /// Whether a mask is necessary to prevent content from unwantingly leaking out of bounds.
        /// Default: false
        /// </summary>
        public bool UseMask
        {
            get => useMask;
            set
            {
                useMask = value;
                if (maskEffect == null)
                {
                    maskEffect = contentContainer.AddEffect(new MaskEffect()).Component;
                    maskEffect.showMaskGraphic = false;
                }
                maskEffect.enabled = value;
            }
        }

        /// <summary>
        /// The speed of parallax movement.
        /// Default: 10
        /// </summary>
        public float ParallaxSpeed
        {
            get => parallaxSpeed;
            set => parallaxSpeed = value;
        }

        /// <summary>
        /// The scale which the contents are resized to achieve parallax effect.
        /// Default: 1.05
        /// </summary>
        public float ParallaxScale
        {
            get => parallaxScale;
            set
            {
                parallaxScale = value;
                ApplyContentScale(value);
                Adjust();
            }
        }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            accelerator = base.InputManager.Accelerator;

            contentContainer = CreateChild<UguiSprite>("content", 0);
            {
                contentContainer.SpriteName = "null";
            }

            InvokeAfterTransformed(1, Adjust);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            GameConfiguration.UseParallax.BindAndTrigger(OnUseParallaxChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            GameConfiguration.UseParallax.OnNewValue -= OnUseParallaxChange;
        }

        /// <summary>
        /// Adapts the content's size to this container to make it fit.
        /// </summary>
        public void Adjust()
        {
            var size = this.Size;
            moveRange.x = (size.x * parallaxScale - size.x) * 0.5f;
            moveRange.y = (size.y * parallaxScale - size.y) * 0.5f;

            contentContainer.Size = size;
        }

        /// <summary>
        /// Applies scaling on the content container.
        /// </summary>
        private void ApplyContentScale(float scale)
        {
            contentContainer.Scale = new Vector3(scale, scale, 1f);
        }

        /// <summary>
        /// Sets whether to use parallax.
        /// </summary>
        private void SetParallax(bool useParallax)
        {
            this.useParallax = useParallax;
            if (useParallax)
            {
                contentContainer.Size = this.Size;
                ApplyContentScale(parallaxScale);
            }
            else
            {
                ApplyContentScale(1f);
                contentContainer.Position = Vector2.zero;
            }
        }

        protected void Update()
        {
            if(!useParallax)
                return;
            if(autoUpdateBounds)
                Adjust();

            var acceleration = accelerator.Acceleration;
            Vector3 pos = contentContainer.Position;
            float delta = Time.deltaTime * parallaxSpeed;
            pos.x = (acceleration.x * -moveRange.x - pos.x) * delta + pos.x;
            pos.y = (acceleration.y * -moveRange.y - pos.y) * delta + pos.y;
            contentContainer.Position = pos;
        }

        /// <summary>
        /// Event called on parallax option change.
        /// </summary>
        private void OnUseParallaxChange(bool useParallax)
        {
            SetParallax(useParallax);
        }
    }
}