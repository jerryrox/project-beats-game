using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare
{
    public class VersionButton : FocusableTrigger, IListItem {

        private const float BaseFocusAlpha = 0.6f;

        private CanvasGroup canvasGroup;
        private IGraphicObject holder;
        private ISprite glow;
        private ISprite bg;
        private ISprite center;
        private ISprite icon;

        private bool isInteractible = true;
        private float curUnfocusAlpha;
        private IPlayableMap myMap;


        public int ItemIndex { get; set; }

        /// <summary>
        /// Whether the button can be interacted by the user.
        /// </summary>
        public bool IsInteractible
        {
            get => isInteractible;
            set
            {
                isInteractible = value;
                RefreshGlowColor();
                SetFocus(ShouldBeFocused);
            }
        }

        /// <summary>
        /// Returns whether this button should be focused with the current state.
        /// </summary>
        private bool ShouldBeFocused => !isInteractible || myMap == MapSelection.Map;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += () =>
            {
                if (myMap != null)
                    MapSelection.SelectMap(myMap);
            };

            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            holder = CreateChild<UguiObject>("holder");
            {
                holder.Anchor = Anchors.Fill;
                holder.RawSize = new Vector2(-12f, -12f);

                glow = holder.CreateChild<UguiSprite>("glow", 0);
                {
                    glow.Anchor = Anchors.Fill;
                    glow.RawSize = new Vector2(56f, 56f);
                    glow.SpriteName = "glow-circle-32";

                    (glow as IRaycastable).IsRaycastTarget = false;
                }
                bg = holder.CreateChild<UguiSprite>("bg", 1);
                {
                    bg.Anchor = Anchors.Fill;
                    bg.RawSize = Vector2.zero;
                    bg.SpriteName = "circle-320";
                }
                center = holder.CreateChild<UguiSprite>("center", 2);
                {
                    center.Anchor = Anchors.Fill;
                    center.RawSize = new Vector2(-10f, -10f);
                    center.SpriteName = "circle-320";
                }
                icon = holder.CreateChild<UguiSprite>("icon", 3);
                {
                    icon.Anchor = Anchors.Fill;
                    icon.RawSize = new Vector2(-12f, -12f);
                    icon.Color = new Color(0.125f, 0.125f, 0.125f);
                }
            }

            // Remove useless sprites.
            hoverSprite.Destroy();
            focusSprite.Destroy();

            focusAni = new Anime();
            focusAni.AnimateFloat(SetFocusAlpha)
                .AddTime(0f, GetFocusAlphaT)
                .AddTime(0.25f, 1f)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat(SetFocusAlpha)
                .AddTime(0f, GetFocusAlphaT)
                .AddTime(0.25f, 0)
                .Build();

            RefreshGlowColor();
            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            BindEvents();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            UnbindEvents();
        }

        public void Setup(IPlayableMap map)
        {
            myMap = map;

            // Set tint color on bg sprite based on map difficulty.
            bg.Color = ColorPreset.GetDifficultyColor(map.Difficulty.Type);

            // Setup ruleset icon.
            var service = ModeManager.GetService(map.PlayableMode);
            if(service == null)
                icon.Active = false;
            else
            {
                icon.Active = true;
                icon.SpriteName = service.IconName;
            }

            // Apply focus only if interactible.
            if (isInteractible)
            {
                SetFocus(ShouldBeFocused, false);
            }
        }

        protected override void OnPointerEntered()
        {
            if(isInteractible)
                base.OnPointerEntered();
        }

        protected override void OnPointerExited()
        {
            if(isInteractible)
                base.OnPointerExited();
        }

        protected override void OnClickTriggered()
        {
            if(isInteractible)
                base.OnClickTriggered();
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.OnMapChange += OnMapChange;
            OnMapChange(MapSelection.Map);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnMapChange -= OnMapChange;
        }

        /// <summary>
        /// Assigns glow sprite color based on current state.
        /// </summary>
        private void RefreshGlowColor()
        {
            glow.Color = isInteractible ? Color.white : Color.black;
            glow.Alpha = 0.25f;
        }

        /// <summary>
        /// Sets focused state visually.
        /// </summary>
        private void SetFocus(bool isFocused, bool animate = true)
        {
            unfocusAni.Stop();
            focusAni.Stop();

            if (!animate)
                SetFocusAlpha(isFocused ? 1 : 0);
            else
            {
                if (isFocused)
                    focusAni.PlayFromStart();
                else
                    unfocusAni.PlayFromStart();
            }
        }

        /// <summary>
        /// Applies alpha value to widgets.
        /// </summary>
        private void SetFocusAlpha(float amount) { canvasGroup.alpha = Mathf.Lerp(BaseFocusAlpha, 1f, amount); }

        /// <summary>
        /// Returns the interpolant value "t" for current canvas group's alpha.
        /// </summary>
        /// <returns></returns>
        private float GetFocusAlphaT() { return Mathf.InverseLerp(BaseFocusAlpha, 1f, canvasGroup.alpha); }

        /// <summary>
        /// Event called on map selection change.
        /// </summary>
        private void OnMapChange(IMap map) => SetFocus(ShouldBeFocused);
    }
}