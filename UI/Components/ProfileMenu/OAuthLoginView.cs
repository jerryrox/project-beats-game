using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public class OAuthLoginView : BaseLoginView {

        private BoxButton loginButton;


        public override float DesiredHeight => 68f;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            loginButton = CreateChild<BoxButton>("login", 4);
            {
                loginButton.Anchor = AnchorType.TopStretch;
                loginButton.Pivot = PivotType.Top;
                loginButton.SetOffsetHorizontal(48f);
                loginButton.Y = -16;
                loginButton.Height = 36f;
                loginButton.Color = colorPreset.Positive;
                loginButton.LabelText = "OAuth log in";

                loginButton.OnTriggered += () =>
                {
                    DoLogin();
                };
            }
        }

        /// <summary>
        /// Starts performing OAuth login.
        /// </summary>
        private void DoLogin() => Api.Request(ApiProvider.OAuth());
    }
}