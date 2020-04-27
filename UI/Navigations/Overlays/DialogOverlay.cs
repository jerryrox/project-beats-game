using System;
using PBGame.UI.Components.Dialog;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class DialogOverlay : BaseOverlay, IDialogOverlay {

        private BlurDisplay blurDisplay;
        private ISprite bgSprite;
        private ISprite blocker;
        private ILabel messageLabel;
        private SelectionHolder selectionHolder;


        protected override int OverlayDepth => ViewDepths.DialogOverlay;

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init(IRootMain root)
        {
            blurDisplay = CreateChild<BlurDisplay>("blur", 0);
            {
                blurDisplay.Anchor = Anchors.Fill;
                blurDisplay.RawSize = Vector2.zero;
            }
            bgSprite = CreateChild<UguiSprite>("bg", 1);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            messageLabel = CreateChild<Label>("message", 2);
            {
                float horizontalOffset = root.Resolution.x / 4;

                messageLabel.Anchor = Anchors.MiddleStretch;
                messageLabel.SetOffsetHorizontal(horizontalOffset);
                messageLabel.Alignment = TextAnchor.LowerCenter;
                messageLabel.WrapText = true;
                messageLabel.FontSize = 26;
                messageLabel.Height = 90f;
                messageLabel.Y = 140f;
            }
            selectionHolder = CreateChild<SelectionHolder>("selection", 3);
            {
                selectionHolder.Anchor = Anchors.MiddleStretch;
                selectionHolder.Pivot = Pivots.Top;
                selectionHolder.SetOffsetHorizontal(0f);
                selectionHolder.Y = 26f;
            }
            blocker = CreateChild<UguiSprite>("blocker", 4);
            {
                blocker.Anchor = Anchors.Fill;
                blocker.RawSize = Vector2.zero;
                blocker.SpriteName = "null";
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
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