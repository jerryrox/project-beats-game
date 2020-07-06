using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.Dialog;
using PBGame.UI.Navigations.Overlays;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class DialogModel : BaseModel {

        private BindableBool isShowing = new BindableBool(false);
        private Bindable<string> message = new Bindable<string>();
        private Bindable<List<DialogOption>> options = new Bindable<List<DialogOption>>();


        /// <summary>
        /// Returns whether the dialog is showing.
        /// </summary>
        public IReadOnlyBindable<bool> IsShowing => isShowing;

        /// <summary>
        /// Returns the message to be displayed on the dialog.
        /// </summary>
        public IReadOnlyBindable<string> Message => message;

        /// <summary>
        /// Returns the list of options on the dialog.
        /// </summary>
        public IReadOnlyBindable<List<DialogOption>> Options => options;

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        /// <summary>
        /// Sets the message to be displayed on the dialog.
        /// </summary>
        public void SetMessage(string message)
        {
            this.message.Value = message;
        }

        /// <summary>
        /// Adds a confirm and a cancel option to the list of options.
        /// </summary>
        public void AddConfirmCancel(Action onConfirm = null, Action onCancel = null)
        {
            AddOptions(new DialogOption[] {
                new DialogOption()
                {
                    Label = "Confirm",
                    Color = ColorPreset.Positive,
                    Callback = onConfirm
                },
                new DialogOption()
                {
                    Label = "Cancel",
                    Color = ColorPreset.Negative,
                    Callback = onCancel
                }
            });
        }

        /// <summary>
        /// Adds the specified option to the options list.
        /// </summary>
        public void AddOption(DialogOption option)
        {
            options.ModifyValue(list => list.Add(option));
        }

        /// <summary>
        /// Adds the specified range of options to the options list.
        /// </summary>
        public void AddOptions(IEnumerable<DialogOption> options)
        {
            this.options.ModifyValue(list => list.AddRange(options));
        }

        /// <summary>
        /// Selects the specified option, followed by dialog closure.
        /// </summary>
        public void SelectOption(DialogOption option)
        {
            if(option == null)
                return;

            option.Invoke();
            CloseDialog();
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        public void CloseDialog()
        {
            OverlayNavigator.Hide<DialogOverlay>();
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();
            isShowing.Value = true;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();
            isShowing.Value = false;
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();
            ResetDialog();
        }

        /// <summary>
        /// Resets the dialog for a clean use.
        /// </summary>
        private void ResetDialog()
        {
            message.Value = "";
            options.ModifyValue(list => list.Clear());
        }
    }
}