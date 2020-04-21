using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Assets.Fonts;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI
{
    /// <summary>
    /// Default input box for PB: Game with default font assignment.
    /// </summary>
    public class InputBox : UguiInputBox {

        [InitWithDependency]
        private void Init(IFontManager fontManager)
        {
            // Support for default font.
            ValueLabel.Font = fontManager.DefaultFont;
            PlaceholderLabel.Font = fontManager.DefaultFont;
        }
    }
}