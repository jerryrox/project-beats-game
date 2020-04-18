using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public abstract class BaseMenuButton : FocusableTrigger {

        protected ISprite iconSprite;


        /// <summary>
        /// Returns the spritename of the icon.
        /// </summary>
        protected abstract string IconName { get; }


        [InitWithDependency]
        private void Init(ISoundPooler soundPooler)
        {
            iconSprite = CreateChild<UguiSprite>();
            {
                if(!string.IsNullOrEmpty(IconName))
                    iconSprite.SpriteName = IconName;
                iconSprite.Size = new Vector2(36f, 36f);
                iconSprite.Alpha = 0.65f;
            }

            UseDefaultHoverAni();
            UseDefaultFocusAni();
        }

        protected override void OnClickTriggered()
        {
            base.OnClickTriggered();
            IsFocused = !IsFocused;
        }
    }
}