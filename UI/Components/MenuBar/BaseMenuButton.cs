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

        /// <summary>
        /// Returns the spritename of the icon.
        /// </summary>
        protected abstract string IconSpritename { get; }


        [InitWithDependency]
        private void Init()
        {
            CreateIconSprite(spriteName: IconSpritename);

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