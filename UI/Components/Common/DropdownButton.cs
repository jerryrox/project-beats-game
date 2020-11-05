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

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.UI.Components.Common
{
    public class DropdownButton : HoverableTrigger, IHasLabel {

        private ILabel label;
        private ISprite backgroundSprite;

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

        /// <summary>
        /// Whether the dropdown data selection should automatically refresh.
        /// Default: true
        /// </summary>
        public bool UseAutoSelect { get; set; } = true;

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        protected override int HoverSpriteDepth => 1;

        [ReceivesDependency]
        private IDropdownProvider DropdownProvider { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += OpenMenu;

            hoverSprite.SpriteName = "circle-16";
            hoverSprite.ImageType = Image.Type.Sliced;

            backgroundSprite = CreateChild<UguiSprite>("background", 0);
            {
                backgroundSprite.Anchor = AnchorType.Fill;
                backgroundSprite.SpriteName = "circle-16";
                backgroundSprite.ImageType = Image.Type.Sliced;
                backgroundSprite.Offset = Offset.Zero;
                backgroundSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            label = CreateChild<Label>("label", 2);
            {
                label.Anchor = AnchorType.Fill;
                label.Offset = new Offset(16f, 0f, 40f, 0f);
                label.Alignment = TextAnchor.MiddleLeft;
                label.FontSize = 16;
            }
            CreateIconSprite(depth: 3, spriteName: "icon-down", size: 20f, alpha: 1f);
            {
                iconSprite.Anchor = AnchorType.Right;
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
                Logger.LogWarning("Dropdown context is empty.");
                return;
            }

            // Get a dropdown menu from provider.
            var dropdown = DropdownProvider.Open(Context);
            // Make the menu appear on the right side of the button.
            Vector2 menuPosition = this.GetPositionAtCorner(PivotType.Right);
            menuPosition -= dropdown.Holder.GetPositionAtCorner(PivotType.TopLeft);
            menuPosition.y += DropdownMenu.ItemSize.y * 0.5f;
            dropdown.PositionMenu(
                myTransform.TransformPoint(menuPosition),
                Space.World
            );
        }

        /// <summary>
        /// Event called on dropdown data selection.
        /// </summary>
        private void OnSelectedData(DropdownData data)
        {
            if(!UseAutoSelect)
                return;

            if (data != null)
                LabelText = data.Text;
            else
                LabelText = "";
        }
    }
}