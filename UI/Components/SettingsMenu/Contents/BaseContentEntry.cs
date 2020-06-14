using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public abstract class BaseContentEntry : UguiSprite {

        /// <summary>
        /// Returns the height of the entry.
        /// </summary>
        protected abstract float EntryHeight { get; }


        [InitWithDependency]
        private void Init()
        {
            Height = EntryHeight;
            Alpha = 0.0625f;
        }

        /// <summary>
        /// Sets entry data to be represented by this object.
        /// </summary>
        public abstract void SetEntryData(SettingsEntryBase entryData);

        /// <summary>
        /// Tries casting the specified entry data as type T and returns it.
        /// If failed, it will throw an exception.
        /// </summary>
        protected T CastEntryData<T>(SettingsEntryBase entryData)
            where T : SettingsEntryBase
        {
            if(entryData is T t)
                return t;
            throw new ArgumentException("entryData is not a type of " + typeof(T).Name);
        }

        /// <summary>
        /// Creates a new label on the entry.
        /// </summary>
        protected ILabel CreateLabel(string text = "", float offsetLeft = 8f, float offsetRight = 8f, float y = -8f, float height = 32f)
        {
            var label = CreateChild<Label>("label");
            label.Anchor = AnchorType.TopStretch;
            label.Pivot = PivotType.TopLeft;
            label.Text = text;
            label.Y = y;
            label.Height = height;
            label.FontSize = 16;
            label.WrapText = true;
            label.SetOffsetHorizontal(offsetLeft, offsetRight);
            return label;
        }
    }
}