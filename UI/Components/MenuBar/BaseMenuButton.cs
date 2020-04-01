using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public class BaseMenuButton : BoxIconTrigger, IMenuButton {

        public event Action OnToggleOn;

        public event Action OnToggleOff;

        private IAnime toggleOnAni;
        private IAnime toggleOffAni;


        public bool IsToggled { get; private set; } = false;

        public ISprite ToggleSprite { get; private set; }


        [InitWithDependency]
        private void Init(ISoundPooler soundPooler)
        {
            ToggleSprite = CreateChild<UguiSprite>("toggle");
            {
                ToggleSprite.Anchor = Anchors.Fill;
                ToggleSprite.RawSize = Vector2.zero;
                ToggleSprite.Alpha = 0f;
            }

            toggleOnAni = new Anime();
            toggleOnAni.AnimateFloat((alpha) => ToggleSprite.Alpha = alpha)
                .AddTime(0f, () => ToggleSprite.Alpha)
                .AddTime(0.5f, 0.25f)
                .Build();

            toggleOffAni = new Anime();
            toggleOffAni.AnimateFloat((alpha) => ToggleSprite.Alpha = alpha)
                .AddTime(0f, () => ToggleSprite.Alpha)
                .AddTime(0.5f, 0f)
                .Build();
        }

        public void SetToggle(bool on)
        {
            IsToggled = on;

            if (on)
            {
                OnToggleOn?.Invoke();

                toggleOffAni.Stop();
                toggleOnAni.PlayFromStart();
            }
            else
            {
                OnToggleOff?.Invoke();

                toggleOnAni.Stop();
                toggleOffAni.PlayFromStart();
            }
        }

        protected override void OnClickTriggered()
        {
            base.OnClickTriggered();
            SetToggle(!IsToggled);
        }
    }
}