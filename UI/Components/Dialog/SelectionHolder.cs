using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Models.Dialog;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Dialog
{
    public class SelectionHolder : UguiObject {

        private ISprite bgSprite;
        private ManagedRecycler<SelectionButton> buttonRecycler;


        [ReceivesDependency]
        private DialogModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            buttonRecycler = new ManagedRecycler<SelectionButton>(CreateButton);

            Height = 0f;

            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Options.BindAndTrigger(OnOptionsChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Options.OnNewValue -= OnOptionsChange;
        }

        /// <summary>
        /// Creates a new dialog option selection button.
        /// </summary>
        private SelectionButton CreateButton()
        {
            var button = CreateChild<SelectionButton>("button");
            {
                button.Anchor = AnchorType.Top;
                button.Pivot = PivotType.Top;
            }
            return button;
        }

        /// <summary>
        /// Builds dialog items using the specified list of options.
        /// </summary>
        private void BuildItems(List<DialogOption> options)
        {
            if(options == null)
                return;

            float height = 0f;
            foreach (var option in options)
            {
                var button = buttonRecycler.GetNext();
                button.Y = height == 0f ? 0f : -height - 2f;
                button.SetOption(option);

                height += button.Height + (height == 0f ? 0f : 2f);
            }
            this.Height = height;
        }

        /// <summary>
        /// Removes all option items.
        /// </summary>
        private void RemoveAll()
        {
            buttonRecycler.ReturnAll();
            Height = 0f;
        }

        /// <summary>
        /// Event called on changes in the list of dialog options.
        /// </summary>
        private void OnOptionsChange(List<DialogOption> options)
        {
            RemoveAll();
            BuildItems(options);
        }
    }
}