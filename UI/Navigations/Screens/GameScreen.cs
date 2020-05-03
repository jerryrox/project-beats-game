using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Screens
{
    public class GameScreen : BaseScreen, IGameScreen {

        protected override int ScreenDepth => ViewDepths.GameScreen;

        protected override bool IsRoot3D => true;


        [InitWithDependency]
        private void Init()
        {
            
        }
    }
}