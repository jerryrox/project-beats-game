using System;
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
    public class DialogOverlay : BaseOverlay, IDialogOverlay {

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

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


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

            if(!IsDerived)
                OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            blocker.Active = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            selectionHolder.RemoveSelections();
        }

        public void SetMessage(string message)
        {
            messageLabel.Text = message;
        }

        public void AddConfirmCancel(Action onConfirm = null, Action onCancel = null)
        {
            AddSelection("Confirm", ColorPreset.Positive, onConfirm);
            AddSelection("Cancel", ColorPreset.Negative, onCancel);
        }

        public void AddSelection(string label, Color color, Action callback = null)
        {
            // Inject closing action.
            Action newCallback = () =>
            {
                callback?.Invoke();
                CloseOverlay();
            };
            selectionHolder.AddSelection(label, color, newCallback);
        }

        /// <summary>
        /// Closes this overlay.
        /// </summary>
        private void CloseOverlay()
        {
            if(HideAnime.IsPlaying)
                return;

            blocker.Active = true;
            OverlayNavigator.Hide(this);
        }
    }
}