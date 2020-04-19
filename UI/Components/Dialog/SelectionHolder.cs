using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Dialog
{
    public class SelectionHolder : UguiObject {

        private ISprite bgSprite;

        private List<SelectionButton> buttons = new List<SelectionButton>();


        [InitWithDependency]
        private void Init()
        {
            Height = 0f;

            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
        }

        /// <summary>
        /// Adds a new selection button using specified values.
        /// </summary>
        public void AddSelection(string label, Color color, Action callback)
        {
            SelectionButton button = CreateChild<SelectionButton>("selection", buttons.Count);
            {
                button.Anchor = Anchors.Top;
                button.Pivot = Pivots.Top;
                button.Y = Height == 0f ? 0f : -Height - 2f;
                
                button.LabelText = label;
                button.BackgroundColor = color;

                if(callback != null)
                    button.OnPointerClick += callback;
            }
            buttons.Add(button);

            Height += Height == 0f ? button.Height : button.Height + 2f;
        }

        /// <summary>
        /// Clears all selection buttons.
        /// </summary>
        public void RemoveSelections()
        {
            buttons.ForEach(b => b.Destroy());
            buttons.Clear();

            Height = 0f;
        }
    }
}