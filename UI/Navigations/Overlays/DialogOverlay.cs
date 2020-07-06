using System;
using PBGame.UI.Models;
using PBGame.UI.Components.Dialog;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class DialogOverlay : BaseOverlay<DialogModel>, IDialogOverlay {

        private BlurDisplay blurDisplay;
        private ISprite bgSprite;
        private Blocker blocker;
        private ILabel messageLabel;
        private SelectionHolder selectionHolder;


        /// <summary>
        /// Returns whether the dialog overlay is derived to another overlay.
        /// </summary>
        protected virtual bool IsDerived => false;

        protected override int ViewDepth => ViewDepths.DialogOverlay;


        [InitWithDependency]
        private void Init(IRootMain root)
        {
            blurDisplay = CreateChild<BlurDisplay>("blur", 0);
            {
                blurDisplay.Anchor = AnchorType.Fill;
                blurDisplay.RawSize = Vector2.zero;
            }
            bgSprite = CreateChild<UguiSprite>("bg", 1);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            messageLabel = CreateChild<Label>("message", 2);
            {
                float horizontalOffset = root.Resolution.x / 4;

                messageLabel.Anchor = AnchorType.MiddleStretch;
                messageLabel.SetOffsetHorizontal(horizontalOffset);
                messageLabel.Alignment = TextAnchor.LowerCenter;
                messageLabel.WrapText = true;
                messageLabel.FontSize = 26;
                messageLabel.Height = 90f;
                messageLabel.Y = 140f;
            }
            selectionHolder = CreateChild<SelectionHolder>("selection", 3);
            {
                selectionHolder.Anchor = AnchorType.MiddleStretch;
                selectionHolder.Pivot = PivotType.Top;
                selectionHolder.SetOffsetHorizontal(0f);
                selectionHolder.Y = 26f;
            }
            blocker = CreateChild<Blocker>("blocker", 4);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            model.IsShowing.BindAndTrigger(OnShowingChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            model.IsShowing.OnNewValue -= OnShowingChange;
        }

        /// <summary>
        /// Event called on isShowing state change.
        /// </summary>
        private void OnShowingChange(bool isShowing)
        {
            blocker.Active = !isShowing;
        }
    }
}