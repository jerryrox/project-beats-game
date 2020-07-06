using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Models.Dialog;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Dialog
{
    public class SelectionHolder : UguiObject {

        private ISprite bgSprite;
        private List<DialogButton> buttons = new List<DialogButton>();


        [ReceivesDependency]
        private DialogModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
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
        /// Builds dialog items using the specified list of options.
        /// </summary>
        private void BuildItems(List<DialogOption> options)
        {
            if(options == null || options.Count == 0)
                return;

            float height = 0f;
            foreach (var option in options)
            {
                DialogButton button = CreateChild<DialogButton>("selection", buttons.Count);
                {
                    button.Anchor = AnchorType.Top;
                    button.Pivot = PivotType.Top;
                    button.Y = Height == 0f ? 0f : -Height - 2f;
                    
                    button.LabelText = option.Label;
                    button.Tint = option.Color;

                    if(option.Callback != null)
                        button.OnTriggered += option.Callback;
                }
                buttons.Add(button);

                height += button.Height + (height == 0f ? 0f : 2f);
            }
            this.Height = height;
        }

        /// <summary>
        /// Removes all option items.
        /// </summary>
        private void RemoveAll()
        {
            buttons.ForEach(b => b.Destroy());
            buttons.Clear();
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