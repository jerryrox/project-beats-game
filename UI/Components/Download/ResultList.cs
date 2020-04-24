using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Download
{
    public class ResultList : UguiListView {

        [InitWithDependency]
        private void Init()
        {
            background.Alpha = 0f;
        }
    }
}