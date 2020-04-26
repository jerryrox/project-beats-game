using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public abstract class BaseSearchFilter : UguiObject {

        protected ILabel label;


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = Anchors.TopStretch;
                label.Pivot = Pivots.Top;
                label.SetOffsetHorizontal(8f);
                label.Y = 0f;
                label.IsBold = true;
                label.FontSize = 17;
                label.Alignment = TextAnchor.UpperLeft;
            }
        }
    }
}