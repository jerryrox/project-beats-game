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
    public class InputBox : UguiInputBox, IInputBox {

        [InitWithDependency]
        private void Init(IFontManager fontManager)
        {
            // Support for default font.
            ValueLabel.Font = fontManager.DefaultFont;
            PlaceholderLabel.Font = fontManager.DefaultFont;
        }
    }
}