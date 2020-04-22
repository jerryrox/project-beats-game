using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class DropdownButton : HoverableTrigger {

        private ILabel label;
        private ISprite backgroundSprite;

        private DropdownMenu dropdownMenu;

        private DropdownContext curContext;


        /// <summary>
        /// Returns the background sprite of the button.
        /// </summary>
        public ISprite BackgroundSprite => backgroundSprite;

        /// <summary>
        /// Return the icon sprite of the button.
        /// </summary>
        public ISprite IconSprite => iconSprite;

        /// <summary>
        /// The dropdown data container context.
        /// </summary>
        public DropdownContext Context
        {
            get => curContext;
            set
            {
                // Unbind from previous context if exists.
                if (curContext != null)
                    curContext.OnSelection -= OnSelectedData;
                // Bind to new context.
                curContext = value;
                if (curContext != null)
                {
                    curContext.OnSelection += OnSelectedData;
                    OnSelectedData(value.Selection);
                }
            }
        }

        protected override int HoverSpriteDepth => 1;

        [ReceivesDependency]
        private IRootMain RootMain { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += OpenMenu;

            hoverSprite.SpriteName = "circle-16";
            hoverSprite.ImageType = Image.Type.Sliced;

            backgroundSprite = CreateChild<UguiSprite>("background", 0);
            {
                backgroundSprite.Anchor = Anchors.Fill;
                backgroundSprite.SpriteName = "circle-16";
                backgroundSprite.ImageType = Image.Type.Sliced;
                backgroundSprite.Offset = Offset.Zero;
                backgroundSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            label = CreateChild<Label>("label", 2);
            {
                label.Anchor = Anchors.Fill;
                label.Offset = new Offset(16f, 0f, 40f, 0f);
                label.Alignment = TextAnchor.MiddleLeft;
                label.FontSize = 16;
            }
            CreateIconSprite(depth: 3, spriteName: "icon-down", size: 20f, alpha: 1f);
            {
                iconSprite.Anchor = Anchors.Right;
                iconSprite.Position = new Vector2(-20f, 0f);
            }

            UseDefaultHoverAni();
        }

        /// <summary>
        /// Opens the dropdown menu.
        /// </summary>
        public void OpenMenu()
        {
            // If empty context, return.
            if (Context == null)
            {
                Debug.LogWarning("DropdownButton.OpenMenu - Context is empty!");
                return;
            }

            // Spawn dropdown menu on the dropdown level.
            if (dropdownMenu == null)
            {
                dropdownMenu = RootMain.CreateChild<DropdownMenu>("dropdown-menu", DepthPresets.DropdownPopup);
                dropdownMenu.Active = false;

                var dropdownCanvas = dropdownMenu.Canvas = dropdownMenu.RawObject.AddComponent<Canvas>();
                dropdownCanvas.overrideSorting = true;
                dropdownCanvas.sortingOrder = DepthPresets.DropdownPopup;

                var raycaster = dropdownMenu.RawObject.AddComponent<GraphicRaycaster>();
                raycaster.ignoreReversedGraphics = true;
            }
            dropdownMenu.OpenMenu(Context);

            // Make the menu appear on the left side of the screen.
            Vector2 menuPosition = Position;
            menuPosition.x += Width * 0.5f + DropdownMenu.ContainerWidth * 0.5f;
            menuPosition.y += -dropdownMenu.HolderSize.y * 0.5f + DropdownMenu.ItemSize.y * 0.5f;
            dropdownMenu.PositionMenu(
                transform.TransformPoint(menuPosition),
                Space.World
            );
        }

        /// <summary>
        /// Event called on dropdown data selection.
        /// </summary>
        private void OnSelectedData(DropdownData data)
        {
            if (data != null)
                label.Text = data.Text;
            else
                label.Text = "";
        }
    }
}