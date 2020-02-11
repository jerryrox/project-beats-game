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
            // Since the existing backgrond sprite IS on the root input box itself, we should override this.
            backgroundSprite.SpriteName = "null";
            backgroundSprite = CreateChild<UguiSprite>("bg", 0);
            {
                backgroundSprite.Anchor = Anchors.Fill;
                backgroundSprite.RawSize = Vector2.zero;
            }

            // Support for default font.
            ValueLabel.Font = fontManager.DefaultFont;
            PlaceholderLabel.Font = fontManager.DefaultFont;
        }
    }
}