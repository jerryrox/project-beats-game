using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.ProfileMenu
{
    public class LoginInput : BasicInput {

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnFocused += (isFocused) =>
            {
                if(isFocused)
                    Tint = ColorPreset.PrimaryFocus;
            };

            UseDefaultFocusAni();
            UseDefaultHoverAni();
        }

        /// <summary>
        /// Simulates invalid input value feedback to user.
        /// </summary>
        public void ShowInvalid() => Tint = ColorPreset.Negative;
    }
}