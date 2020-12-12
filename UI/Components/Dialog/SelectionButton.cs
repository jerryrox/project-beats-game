using PBGame.UI.Models;
using PBGame.UI.Models.Dialog;
using PBGame.UI.Components.Common;
using PBFramework.Dependencies;
using PBFramework.Allocation.Recyclers;
using UnityEngine;

namespace PBGame.UI.Components.Dialog
{
    public class SelectionButton : DialogButton, IRecyclable<SelectionButton> {

        private DialogOption option;


        /// <summary>
        /// Current dialog option instance associated with the button.
        /// </summary>
        public DialogOption Option => option;

        IRecycler<SelectionButton> IRecyclable<SelectionButton>.Recycler { get; set; }

        [ReceivesDependency]
        private DialogModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += () =>
            {
                Model.SelectOption(Option);
            };
        }

        /// <summary>
        /// Sets the dialog option of the button.
        /// </summary>
        public void SetOption(DialogOption option)
        {
            this.option = option;
            LabelText = option?.Label;
            Tint = option?.Color ?? Color.white;
        }

        void IRecyclable.OnRecycleNew()
        {
            Active = true;
        }

        void IRecyclable.OnRecycleDestroy()
        {
            Active = false;
            option = null;
        }
    }
}