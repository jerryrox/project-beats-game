using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class Header : UguiObject, IHeader {

        private IProfileImage profileImage;
        private ILabel nickname;


        /// <summary>
        /// Returns the osu api from manager.
        /// </summary>
        private IApi OsuApi => ApiManager.GetApi(ApiProviders.Osu);

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            profileImage = CreateChild<ProfileImage>("profile", 0);
            {
                profileImage.Anchor = Anchors.Bottom;
                profileImage.Size = new Vector2(100f, 100f);
                profileImage.Y = 94f;
            }
            nickname = CreateChild<Label>("nickname", 1);
            {
                nickname.Anchor = Anchors.BottomStretch;
                nickname.RawWidth = -64f;
                nickname.Y = 24f;
                nickname.Height = 30;
                nickname.Alignment = TextAnchor.MiddleCenter;
                nickname.IsBold = true;
                nickname.WrapText = true;
                nickname.FontSize = 18;
            }
        }
        
        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            nickname.Text = OsuApi.User.Value.Username;
        }
    }
}